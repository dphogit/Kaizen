import { useMeContext } from "../features/auth/stores/me-context";
import { useLogoutMutation } from "../features/auth/api";
import { notifications } from "@mantine/notifications";
import { Button, Menu } from "@mantine/core";
import { IconChevronDown, IconLogout, IconUser } from "@tabler/icons-react";

export default function UserMenu() {
  const me = useMeContext();

  const logoutMutation = useLogoutMutation();

  function handleLogoutClick() {
    logoutMutation.mutate(undefined, {
      onError: () => {
        notifications.show({
          title: "Logout Failed",
          message: "An error occurred, please try again later.",
          color: "red",
          position: "top-center",
        });
      },
    });
  }

  return (
    <Menu width={200} position="bottom-end" shadow="xl">
      <Menu.Target>
        <Button
          radius="sm"
          variant="transparent"
          rightSection={<IconChevronDown size={14} stroke={3} />}
          color="white"
          px={0}
        >
          {me.email}
        </Button>
      </Menu.Target>

      <Menu.Dropdown>
        <Menu.Label>Settings</Menu.Label>
        <Menu.Item leftSection={<IconUser size={14} />} disabled>
          Profile (Coming Soon)
        </Menu.Item>
        <Menu.Item
          leftSection={<IconLogout size={14} />}
          onClick={handleLogoutClick}
        >
          Log Out
        </Menu.Item>
      </Menu.Dropdown>
    </Menu>
  );
}
