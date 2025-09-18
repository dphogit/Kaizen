import type { ReactNode } from "react";
import { AppShell, Burger, Button, Group, Text, Title } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { IconLogout } from "@tabler/icons-react";
import { useMeContext } from "@/features/auth/stores/me-context";
import { useLogoutMutation } from "@/features/auth/api";
import { notifications } from "@mantine/notifications";

export default function AppLayout(props: { children: ReactNode }) {
  const [opened, handlers] = useDisclosure();

  const me = useMeContext();

  return (
    <AppShell
      header={{ height: 60 }}
      navbar={{ width: 300, breakpoint: "sm", collapsed: { mobile: !opened } }}
      padding="xl"
    >
      <AppShell.Header>
        <Group h="100%" px="md" justify="space-between">
          <Burger
            opened={opened}
            onClick={handlers.toggle}
            hiddenFrom="sm"
            size="sm"
          />

          <Title>Kaizen🔥</Title>
          <Group>
            <Text>{me.email}</Text>
            <LogoutButton />
          </Group>
        </Group>
      </AppShell.Header>

      <AppShell.Navbar p="md">Navbar links</AppShell.Navbar>

      <AppShell.Main>{props.children}</AppShell.Main>
    </AppShell>
  );
}

function LogoutButton() {
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
    <Button
      variant="default"
      rightSection={<IconLogout size={14} />}
      onClick={handleLogoutClick}
    >
      Logout
    </Button>
  );
}
