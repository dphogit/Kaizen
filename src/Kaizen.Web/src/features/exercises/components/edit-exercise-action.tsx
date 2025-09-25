import { ActionIcon, Modal } from "@mantine/core";
import { IconEdit } from "@tabler/icons-react";
import type { Exercise } from "../types";
import { useDisclosure } from "@mantine/hooks";
import ExerciseForm from "./exercise-form";

type EditExerciseActionProps = {
  exercise: Exercise;
};

export default function EditExerciseAction(props: EditExerciseActionProps) {
  const [opened, { open, close }] = useDisclosure();

  return (
    <>
      <Modal opened={opened} onClose={close} title="Edit Exercise">
        <ExerciseForm exercise={props.exercise} onSaveSuccess={close} />
      </Modal>

      <ActionIcon variant="subtle" color="gray" radius="sm" onClick={open}>
        <IconEdit stroke={1.5} />
      </ActionIcon>
    </>
  );
}
