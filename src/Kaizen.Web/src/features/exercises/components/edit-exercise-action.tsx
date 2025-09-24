import { ActionIcon } from "@mantine/core";
import { IconEdit } from "@tabler/icons-react";

export default function EditExerciseAction() {
  return (
    <ActionIcon variant="subtle" color="gray" radius="sm">
      <IconEdit stroke={1.5} />
    </ActionIcon>
  );
}
