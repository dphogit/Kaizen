import type { Exercise } from "../types";
import { ActionIcon, Badge, Group, Paper, Text } from "@mantine/core";
import { IconEdit } from "@tabler/icons-react";

type ExerciseCardProps = {
  exercise: Exercise;
};

export default function ExerciseCard(props: ExerciseCardProps) {
  return (
    <Paper p="md" shadow="xs">
      <Group justify="space-between" align="center">
        <Text size="24px">{props.exercise.name}</Text>
        <EditExerciseAction />
      </Group>
      <Group gap="xs" mt="md">
        {props.exercise.muscleGroups.map((mg) => (
          <Badge key={mg.code} size="xs" color="var(--mantine-color-dark-4)">
            {mg.name}
          </Badge>
        ))}
      </Group>
    </Paper>
  );
}

function EditExerciseAction() {
  return (
    <ActionIcon variant="subtle" color="gray" radius="sm">
      <IconEdit stroke={1.5} />
    </ActionIcon>
  );
}
