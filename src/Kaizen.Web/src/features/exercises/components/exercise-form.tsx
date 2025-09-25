import {
  Alert,
  Button,
  Center,
  Group,
  List,
  Loader,
  MultiSelect,
  TextInput,
} from "@mantine/core";
import {
  useEditExerciseMutation,
  useExerciseMutation,
  useMuscleGroupsQuery,
} from "../api";
import { isNotEmpty, useForm } from "@mantine/form";
import type { Exercise, UpsertExercise } from "../types";
import { useState } from "react";
import { IconDeviceFloppy } from "@tabler/icons-react";
import { HttpStatusCode, isAxiosError } from "axios";
import { getValidationErrors } from "../../../lib/api-client";

export type ExerciseFormProps = {
  exercise?: Exercise;
  onSaveSuccess?: () => void;
};

const emptyValues: UpsertExercise = {
  name: "",
  muscleGroupCodes: [],
};

const toFormValues = (exercise: Exercise): UpsertExercise => ({
  name: exercise.name,
  muscleGroupCodes: exercise.muscleGroups.map((mg) => mg.code),
});

export default function ExerciseForm(props: ExerciseFormProps) {
  const [clientErrors, setClientErrors] = useState<string[] | null>(null);
  const [serverError, setServerError] = useState<string | null>(null);

  const isEditMode = Boolean(props.exercise);
  const initialValues: UpsertExercise = props.exercise
    ? toFormValues(props.exercise)
    : emptyValues;

  const form = useForm({
    mode: "uncontrolled",
    initialValues,
    validate: {
      name: (value) => isNotEmpty("Name cannot be empty")(value),
      muscleGroupCodes: isNotEmpty("Must select at least one muscle group"),
    },
  });

  const muscleGroupsQuery = useMuscleGroupsQuery();
  const createExerciseMutation = useExerciseMutation();
  const editExerciseMutation = useEditExerciseMutation();

  function submitEdit(values: typeof form.values) {
    const id = props.exercise?.id;

    if (!id) {
      // Should never occur as if editing there is an exercise (has id)
      throw new Error("No id for exercise to update");
    }

    if (!form.isDirty()) {
      // No changes - give an illusion of successful save.
      props.onSaveSuccess?.();
      return;
    }

    editExerciseMutation.mutate(
      { id, payload: values },
      {
        onSuccess: () => {
          props.onSaveSuccess?.();
        },
        onError: (err) => {
          if (isAxiosError(err) && err.status === HttpStatusCode.BadRequest) {
            setClientErrors(getValidationErrors(err));
            return;
          }

          setServerError(
            "Could not update exercise due to server error. Try again later.",
          );
        },
      },
    );
  }

  function submitCreate(values: typeof form.values) {
    createExerciseMutation.mutate(values, {
      onSuccess: () => {
        props.onSaveSuccess?.();
      },
      onError: (err) => {
        if (isAxiosError(err) && err.status === HttpStatusCode.BadRequest) {
          setClientErrors(getValidationErrors(err));
          return;
        }

        setServerError(
          "Could not create exercise due to server error. Try again later.",
        );
      },
    });
  }

  function handleSubmit(values: typeof form.values) {
    if (isEditMode) {
      submitEdit(values);
      return;
    }

    submitCreate(values);
  }

  if (muscleGroupsQuery.data) {
    const isPending =
      createExerciseMutation.isPending || editExerciseMutation.isPending;

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
        <form onSubmit={form.onSubmit(handleSubmit)}>
          <TextInput
            required
            label="Name"
            placeholder="e.g. Bicep Curl"
            mb="sm"
            disabled={isPending}
            key={form.key("name")}
            {...form.getInputProps("name")}
          />
          <MultiSelect
            required
            label="Muscle Groups"
            placeholder="Pick Muscle Groups"
            searchable
            nothingFoundMessage="Nothing found..."
            disabled={isPending}
            data={muscleGroupsQuery.data.map((mg) => ({
              value: mg.code,
              label: mg.name,
            }))}
            key={form.key("muscleGroupCodes")}
            {...form.getInputProps("muscleGroupCodes")}
          />
          <Group justify="flex-end" mt="xl">
            <Button
              type="submit"
              disabled={!form.isValid()}
              loading={isPending}
              rightSection={<IconDeviceFloppy stroke={1.5} />}
            >
              Save Exercise
            </Button>
          </Group>
        </form>
      </>
    );
  }

  if (muscleGroupsQuery.error) {
    return (
      <Alert variant="light" color="red" title="Unexpected Error">
        Could not setup form. Try again later.
      </Alert>
    );
  }

  return (
    <Center>
      <Loader />
    </Center>
  );
}
