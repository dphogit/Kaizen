import { type ReactNode, useState } from "react";
import { type TransformedValues, useForm } from "@mantine/form";
import {
  ActionIcon,
  Alert,
  Button,
  Fieldset,
  Flex,
  Group,
  List,
  NumberInput,
  Select,
  Text,
  TextInput,
} from "@mantine/core";
import { DateTimePicker } from "@mantine/dates";
import { DATETIME_FORMAT } from "@/lib/dates";
import { randomId } from "@mantine/hooks";
import { IconPlus, IconX } from "@tabler/icons-react";
import { z } from "zod";
import { zod4Resolver } from "mantine-form-zod-resolver";
import type { Exercise } from "../../exercises/types";
import type { MeasurementUnit, Workout } from "../types";
import dayjs from "dayjs";
import { useUpdateWorkoutMutation, useWorkoutMutation } from "../api";
import { useNavigate } from "react-router";
import { AppRoutes } from "../../../routes";
import { notifications } from "@mantine/notifications";
import { HttpStatusCode, isAxiosError } from "axios";
import { getValidationErrors } from "../../../lib/api-client";

const workoutSetFormSchema = z.object({
  id: z.string(),
  exerciseId: z.string().trim().min(1, { error: "Exercise is required" }),
  repetitions: z.int().min(0),
  quantity: z.number().min(0),
  measurementUnitCode: z.string().min(1, { error: "Unit is required" }),
});

const workoutFormSchema = z.object({
  name: z.string().min(1, { error: "Workout name is required" }),
  performedAt: z.string(),
  sets: workoutSetFormSchema
    .array()
    .min(1, { error: "Workouts must contain at least one set." }),
});

type WorkoutSetFormValues = z.infer<typeof workoutSetFormSchema>;
type WorkoutFormValues = z.infer<typeof workoutFormSchema>;

function formatSetKeyPath(index: number, property: keyof WorkoutSetFormValues) {
  return `sets.${index}.${property}`;
}

function WorkoutSetColumnHeader(props: { flex: number; children: ReactNode }) {
  return (
    <Text fw={500} flex={props.flex} size="sm">
      {props.children}
    </Text>
  );
}

function exercisesNameComparer(a: Exercise, b: Exercise) {
  if (a.name === b.name) return 0;
  return a.name < b.name ? -1 : 1;
}

function unitsNameComparer(a: MeasurementUnit, b: MeasurementUnit) {
  if (a.name === b.name) return 0;
  return a.name < b.name ? -1 : 1;
}

function getFormInitialValues(workout?: Workout): WorkoutFormValues {
  if (!workout) {
    return {
      name: "",
      performedAt: new Date().toISOString(),
      sets: [],
    };
  }

  const sets = workout.sets.map((set) => ({
    ...set,
    id: set.id.toString(),
    exerciseId: set.exerciseId.toString(),
  }));

  return {
    name: workout.name,
    performedAt: workout.performedAt,
    sets,
  };
}

const WorkoutSetColumnFlex = {
  Exercise: 5,
  Reps: 1,
  QtyAndUnit: 2,
} as const;

type WorkoutFormProps = {
  exercises: Exercise[];
  units: MeasurementUnit[];
  workout?: Workout;
};

export default function WorkoutForm(props: WorkoutFormProps) {
  const [clientErrors, setClientErrors] = useState<string[] | null>(null);
  const [serverError, setServerError] = useState<string | null>(null);

  const isEditMode = Boolean(props.workout);

  const form = useForm({
    mode: "uncontrolled",
    initialValues: getFormInitialValues(props.workout),
    validate: zod4Resolver(workoutFormSchema),

    transformValues: (values) => {
      const performedAt = dayjs(values.performedAt).toISOString();
      const sets = values.sets.map((s) => ({
        ...s,
        exerciseId: Number(s.exerciseId),
      }));

      return {
        ...values,
        performedAt,
        sets,
      };
    },
  });

  const workoutMutation = useWorkoutMutation();
  const updateWorkoutMutation = useUpdateWorkoutMutation();
  const navigate = useNavigate();

  function submitValues(values: TransformedValues<typeof form>) {
    if (isEditMode) {
      submitEdit(values);
      return;
    }

    submitAdd(values);
  }

  function submitEdit(values: TransformedValues<typeof form>) {
    const id = props.workout?.id;

    if (!id) {
      throw new Error("No id for workout to update");
    }

    updateWorkoutMutation.mutate(
      { id, workout: values },
      {
        onSuccess: onSubmitSuccess,
        onError: onSubmitError,
      },
    );
  }

  function submitAdd(values: TransformedValues<typeof form>) {
    workoutMutation.mutate(values, {
      onSuccess: onSubmitSuccess,
      onError: onSubmitError,
    });
  }

  function onSubmitSuccess() {
    const message = isEditMode
      ? "Your workout was updated successfully."
      : "Well done on completing your workout";

    notifications.show({
      title: "Workout Saved",
      message,
      color: "teal",
    });

    navigate(AppRoutes.Home);
  }

  function onSubmitError(err: Error) {
    if (isAxiosError(err) && err.status === HttpStatusCode.BadRequest) {
      setClientErrors(getValidationErrors(err));
      return;
    }

    setServerError(
      "Could not save workout due to server error. Try again later.",
    );
  }

  function addSetField() {
    const set: WorkoutSetFormValues = {
      id: randomId(),
      exerciseId: "",
      measurementUnitCode: "",
      quantity: 0,
      repetitions: 0,
    };

    form.insertListItem("sets", set);
  }

  const formValues = form.getValues();

  const exercises = props.exercises.sort(exercisesNameComparer);
  const units = props.units.sort(unitsNameComparer);

  return (
    <>
      {clientErrors && (
        <Alert variant="light" color="red" title="Validation Errors">
          <List>
            {clientErrors.map((err) => (
              <List.Item key={err}>{err}</List.Item>
            ))}
          </List>
        </Alert>
      )}
      {serverError && (
        <Alert variant="light" color="red" title="Unexpected Error">
          {serverError}
        </Alert>
      )}
      <form
        onSubmit={form.onSubmit(submitValues)}
        style={{ position: "relative" }}
      >
        <Flex w="100%" columnGap="md">
          <TextInput
            label="Name"
            placeholder="e.g. Full Body Workout"
            key={form.key("name")}
            {...form.getInputProps("name")}
            flex={2}
          />
          <DateTimePicker
            label="Performed At"
            key={form.key("performedAt")}
            valueFormat={DATETIME_FORMAT}
            withSeconds={false}
            flex={1}
            timePickerProps={{
              withDropdown: true,
              format: "12h",
            }}
            {...form.getInputProps("performedAt")}
          />
        </Flex>

        <Fieldset
          legend="Sets"
          my="md"
          styles={{
            legend: { fontWeight: 500, textAlign: "center" },
          }}
        >
          {formValues.sets.length > 0 && (
            <Flex mt="6px" mb="xs" columnGap="xs" justify="space-between">
              <WorkoutSetColumnHeader flex={WorkoutSetColumnFlex.Exercise}>
                Exercise
              </WorkoutSetColumnHeader>
              <WorkoutSetColumnHeader flex={WorkoutSetColumnFlex.Reps}>
                Reps
              </WorkoutSetColumnHeader>
              <WorkoutSetColumnHeader flex={WorkoutSetColumnFlex.QtyAndUnit}>
                Qty & Unit
              </WorkoutSetColumnHeader>
              {/* Same width as size of the ActionIcon button */}
              <div style={{ width: 18 }} />
            </Flex>
          )}
          {formValues.sets.map((item, index) => (
            <Flex
              key={item.id}
              mb="md"
              columnGap="xs"
              align="center"
              justify="space-between"
            >
              <Select
                placeholder="Select Exercise"
                size="xs"
                flex={WorkoutSetColumnFlex.Exercise}
                searchable
                data={exercises.map((e) => ({
                  value: e.id.toString(),
                  label: e.name,
                }))}
                key={form.key(formatSetKeyPath(index, "exerciseId"))}
                {...form.getInputProps(formatSetKeyPath(index, "exerciseId"))}
              />
              <NumberInput
                min={0}
                placeholder="0"
                allowDecimal={false}
                hideControls
                flex={WorkoutSetColumnFlex.Reps}
                size="xs"
                key={form.key(formatSetKeyPath(index, "repetitions"))}
                {...form.getInputProps(formatSetKeyPath(index, "repetitions"))}
              />
              <Flex flex={WorkoutSetColumnFlex.QtyAndUnit}>
                <NumberInput
                  min={0}
                  placeholder="0"
                  hideControls
                  size="xs"
                  key={form.key(formatSetKeyPath(index, "quantity"))}
                  {...form.getInputProps(formatSetKeyPath(index, "quantity"))}
                />
                <Select
                  placeholder="kg"
                  data={units.map((u) => u.code)}
                  searchable
                  key={form.key(formatSetKeyPath(index, "measurementUnitCode"))}
                  size="xs"
                  {...form.getInputProps(
                    formatSetKeyPath(index, "measurementUnitCode"),
                  )}
                />
              </Flex>
              <ActionIcon
                onClick={() => form.removeListItem("sets", index)}
                variant="transparent"
                color="gray"
                size="xs"
              >
                <IconX />
              </ActionIcon>
            </Flex>
          ))}
          <Button
            type="button"
            variant="default"
            onClick={addSetField}
            leftSection={<IconPlus size={18} />}
          >
            Add Set
          </Button>
        </Fieldset>

        <Group justify="flex-end">
          <Button
            type="submit"
            disabled={!form.isValid() || (isEditMode && !form.isDirty())}
            loading={workoutMutation.isPending}
          >
            Save Workout
          </Button>
        </Group>
      </form>
    </>
  );
}
