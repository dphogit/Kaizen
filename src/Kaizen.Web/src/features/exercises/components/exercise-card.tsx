import type { Exercise } from "../types";
import { Badge, Group, Paper, Text } from "@mantine/core";
import EditExerciseAction from "./edit-exercise-action";

type ExerciseCardProps = {
  exercise: Exercise;
};

export default function ExerciseCard(props: ExerciseCardProps) {
  return (
    <Paper>
      <Group justify="space-between" align="center">
        <Text size="24px">{props.exercise.name}</Text>
        <EditExerciseAction />
      </Group>
      <Group gap="xs" mt="sm">
        {props.exercise.muscleGroups.map((mg) => (
          <Badge key={mg.code} size="xs" color="blue" radius="sm">
            {mg.name}
          </Badge>
        ))}
      </Group>
    </Paper>
  );
}
