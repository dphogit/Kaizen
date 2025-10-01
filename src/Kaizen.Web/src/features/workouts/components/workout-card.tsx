import { Group, Paper, Stack, Text, Title } from "@mantine/core";
import type { Workout } from "../types";
import { formatDate } from "@/lib/dates";

type WorkoutCardProps = {
  workout: Workout;
};

export default function WorkoutCard({ workout }: WorkoutCardProps) {
  return (
    <Paper>
      <Group justify="space-between" align="flex-start">
        <Stack gap="4px">
          <Title order={2}>{workout.name}</Title>
          <Text c="dimmed">{formatDate(workout.performedAt)}</Text>
        </Stack>
        <div>Actions</div>
      </Group>
      <div>Collapsible Sets</div>
    </Paper>
  );
}
