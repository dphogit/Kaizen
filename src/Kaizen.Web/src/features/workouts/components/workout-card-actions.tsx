import type { Workout } from "../types";
import { useDisclosure } from "@mantine/hooks";
import { ActionIcon, Menu } from "@mantine/core";
import { IconDots, IconEdit, IconTrash } from "@tabler/icons-react";
import DeleteWorkoutModal from "./delete-workout-modal";
import { useNavigate } from "react-router";
import { AppRoutes } from "../../../routes";

type WorkoutCardActionsProps = {
  workout: Workout;
};

export default function WorkoutCardActions(props: WorkoutCardActionsProps) {
  const [opened, { open, close }] = useDisclosure();
  const navigate = useNavigate();

  function onEditClick() {
    navigate(AppRoutes.Workouts.Edit(props.workout.id));
  }

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
          <Menu.Item leftSection={<IconEdit size={16} />} onClick={onEditClick}>
            Edit
          </Menu.Item>
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
