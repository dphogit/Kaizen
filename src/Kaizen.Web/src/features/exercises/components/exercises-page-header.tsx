import { Button, Group, Modal, Title } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import ExerciseForm from "./exercise-form";

export default function ExercisesPageHeader() {
  const [opened, { open, close }] = useDisclosure();

  return (
    <>
      <Modal
        opened={opened}
        onClose={close}
        title="Add an Exercise"
        size="lg"
        styles={{
          title: {
            fontSize: "var(--mantine-font-size-xl)",
          },
        }}
      >
        <ExerciseForm onSaveSuccess={close} />
      </Modal>

      <Group justify="space-between" mb="lg">
        <Title>Manage Exercises</Title>
        <Button onClick={open}>Add</Button>
      </Group>
    </>
  );
}
