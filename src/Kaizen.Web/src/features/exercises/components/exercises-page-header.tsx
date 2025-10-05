import { Button, Group, Modal, Title } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import ExerciseForm from "./exercise-form";
import { IconPlus } from "@tabler/icons-react";

export default function ExercisesPageHeader() {
  const [opened, { open, close }] = useDisclosure();

  return (
    <>
      <Modal opened={opened} onClose={close} title="Add an Exercise">
        <ExerciseForm onSaveSuccess={close} />
      </Modal>

      <Group justify="space-between" mb="lg">
        <Title>Manage Exercises</Title>
        <Button onClick={open} leftSection={<IconPlus size={18} />}>
          Add
        </Button>
      </Group>
    </>
  );
}
