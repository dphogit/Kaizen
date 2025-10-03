import type { Workout } from "../types";
import { useDeleteWorkoutMutation } from "../api";
import { Alert, Button, Group, Modal, Text } from "@mantine/core";

type DeleteWorkoutModalProps = {
  workout: Workout;
  opened: boolean;
  onClose: () => void;
};

export default function DeleteWorkoutModal(props: DeleteWorkoutModalProps) {
  const mutation = useDeleteWorkoutMutation();

  function deleteWorkout() {
    mutation.mutate(props.workout.id, { onSuccess: close });
  }

  return (
    <Modal
      opened={props.opened}
      onClose={props.onClose}
      title="Delete Workout?"
      size="md"
    >
      {mutation.isError && (
        <Alert color="red" title="Error" mb="md">
          Failed to delete workout. Please try again later.
        </Alert>
      )}
      <Text>Delete this workout? This action cannot be undone.</Text>
      <Group justify="flex-end" mt="xl">
        <Button variant="default" onClick={close} disabled={mutation.isPending}>
          Cancel
        </Button>
        <Button
          variant="filled"
          color="red"
          onClick={deleteWorkout}
          loading={mutation.isPending || mutation.isError}
        >
          Delete
        </Button>
      </Group>
    </Modal>
  );
}
