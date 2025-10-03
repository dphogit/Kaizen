import {
  Button,
  Collapse,
  Group,
  Paper,
  Stack,
  Table,
  Text,
  Title,
} from "@mantine/core";
import { formatDate } from "@/lib/dates";
import type { Workout } from "../types";
import WorkoutCardActions from "./workout-card-actions";
import { useDisclosure } from "@mantine/hooks";
import { IconChevronDown, IconChevronUp } from "@tabler/icons-react";

type WorkoutCardProps = {
  workout: Workout;
};

export default function WorkoutCard({ workout }: WorkoutCardProps) {
  const [opened, { toggle }] = useDisclosure();

  return (
    <Paper>
      <Group justify="space-between" align="flex-start">
        <Stack gap="4px">
          <Title order={2}>{workout.name}</Title>
          <Text c="dimmed">{formatDate(workout.performedAt)}</Text>
        </Stack>

        <WorkoutCardActions workout={workout} />
      </Group>

      <Collapse in={opened}>
        <Table tabularNums my="sm">
          <Table.Thead>
            <Table.Tr>
              <Table.Th pl={0}>Exercise</Table.Th>
              <Table.Th ta="center">Reps</Table.Th>
              <Table.Th ta="center">Qty</Table.Th>
              <Table.Th ta="center">Unit</Table.Th>
            </Table.Tr>
          </Table.Thead>
          <Table.Tbody>
            {workout.sets.map((s) => (
              <Table.Tr key={s.id}>
                <Table.Td w="70%" pl={0}>
                  {s.exerciseName}
                </Table.Td>
                <Table.Td w="10%" ta="center">
                  {s.repetitions}
                </Table.Td>
                <Table.Td w="10%" ta="center">
                  {s.quantity}
                </Table.Td>
                <Table.Td w="10%" ta="center">
                  {s.measurementUnitCode}
                </Table.Td>
              </Table.Tr>
            ))}
          </Table.Tbody>
        </Table>
      </Collapse>

      <Button
        fullWidth
        size="compact-sm"
        variant="subtle"
        color="gray"
        onClick={toggle}
        rightSection={
          opened ? <IconChevronUp size={18} /> : <IconChevronDown size={18} />
        }
        styles={{ section: { marginTop: 2, marginInlineStart: 6 } }}
      >
        {opened ? "Collapse" : "Details"}
      </Button>
    </Paper>
  );
}
