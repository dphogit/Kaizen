import {
  Alert,
  Button,
  Center,
  Group,
  Loader,
  MultiSelect,
  TextInput,
} from "@mantine/core";
import { useExerciseMutation, useMuscleGroupsQuery } from "../api";
import { isNotEmpty, useForm } from "@mantine/form";
import type { CreateExercise } from "../types";
import { useState } from "react";

export type ExerciseFormProps = {
  onSaveSuccess?: () => void;
};

export default function ExerciseForm(props: ExerciseFormProps) {
  const [serverError, setServerError] = useState<string | null>(null);

  const form = useForm<CreateExercise>({
    mode: "uncontrolled",
    initialValues: {
      name: "",
      muscleGroupCodes: [],
    },
    validate: {
      name: isNotEmpty("Name cannot be empty"),
      muscleGroupCodes: isNotEmpty("Must select at least one muscle group"),
    },
  });

  const muscleGroupsQuery = useMuscleGroupsQuery();
  const exerciseMutation = useExerciseMutation();

  function handleSubmit(values: typeof form.values) {
    exerciseMutation.mutate(values, {
      onSuccess: () => {
        props.onSaveSuccess && props.onSaveSuccess();
      },
      onError: () => {
        setServerError("Could not create exercise due to server error.");
      },
    });
  }

  if (muscleGroupsQuery.data) {
    if (serverError) {
      return (
        <Alert variant="light" color="red" title="Unexpected Error">
          {serverError}
        </Alert>
      );
    }

    return (
      <form onSubmit={form.onSubmit(handleSubmit)} onReset={form.onReset}>
        <TextInput
          required
          label="Name"
          placeholder="e.g. Bicep Curl"
          mb="sm"
          disabled={exerciseMutation.isPending}
          key={form.key("name")}
          {...form.getInputProps("name")}
        />
        <MultiSelect
          required
          label="Muscle Groups"
          placeholder="Pick Muscle Groups"
          searchable
          nothingFoundMessage="Nothing found..."
          disabled={exerciseMutation.isPending}
          data={muscleGroupsQuery.data.map((mg) => ({
            value: mg.code,
            label: mg.name,
          }))}
          key={form.key("muscleGroupCodes")}
          {...form.getInputProps("muscleGroupCodes")}
        />
        <Group justify="flex-end" mt="xl">
          <Button type="reset" variant="default">
            Clear
          </Button>
          <Button
            type="submit"
            disabled={!form.isValid()}
            loading={exerciseMutation.isPending}
          >
            Add Exercise
          </Button>
        </Group>
      </form>
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
