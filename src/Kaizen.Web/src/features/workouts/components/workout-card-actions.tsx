import type { Workout } from "../types";
import { useDisclosure } from "@mantine/hooks";
import { ActionIcon, Menu } from "@mantine/core";
import { IconDots, IconEdit, IconTrash } from "@tabler/icons-react";
import DeleteWorkoutModal from "./delete-workout-modal";

type WorkoutCardActionsProps = {
  workout: Workout
}

export default function WorkoutCardActions(props: WorkoutCardActionsProps) {
  const [opened, { open, close }] = useDisclosure();

  return (
    <>
      <Menu>
        <Menu.Target>
          <ActionIcon aria-label="options">
            <IconDots />
          </ActionIcon>
        </Menu.Target>

        <Menu.Dropdown>
          <Menu.Label>Options</Menu.Label>
          <Menu.Item leftSection={<IconEdit size={16} />}>Edit</Menu.Item>
          <Menu.Item
            leftSection={<IconTrash size={16} color="red" />}
            c="red"
            onClick={open}
          >
            Delete
          </Menu.Item>
        </Menu.Dropdown>
      </Menu>

      <DeleteWorkoutModal
        workout={props.workout}
        opened={opened}
        onClose={close}
      />
    </>
  );
}