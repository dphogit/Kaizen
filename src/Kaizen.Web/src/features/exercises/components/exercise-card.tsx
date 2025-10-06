import type { Exercise } from "../types";
import { Badge, Group, Paper, Title } from "@mantine/core";
import EditExerciseAction from "./edit-exercise-action";

type ExerciseCardProps = {
  exercise: Exercise;
};

export default function ExerciseCard(props: ExerciseCardProps) {
  return (
    <Paper h="100%">
      <Group justify="space-between" align="center">
        <Title order={2} size="18px" lineClamp={1}>
          {props.exercise.name}
        </Title>
        <EditExerciseAction exercise={props.exercise} />
      </Group>
      <Group gap="xs" mt="sm">
        {props.exercise.muscleGroups.map((mg) => (
          <Badge
            key={mg.code}
            size="xs"
            color="blue"
            radius="sm"
            variant="light"
          >
            {mg.name}
          </Badge>
        ))}
      </Group>
    </Paper>
  );
}
