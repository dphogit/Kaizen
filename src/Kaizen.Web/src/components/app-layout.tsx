import { AppShell, Button, Container, Group, Menu, Title } from "@mantine/core";
import { IconChevronDown, IconLogout, IconUser } from "@tabler/icons-react";
import { useMeContext } from "@/features/auth/stores/me-context";
import { useLogoutMutation } from "@/features/auth/api";
import { notifications } from "@mantine/notifications";
import { NavLink as ReactRouterNavLink, Outlet } from "react-router";
import { AppRoutes } from "../routes";
import type { ReactNode } from "react";

function PageContainer(props: { children: ReactNode }) {
  return (
    <Container maw="1280px" px="xl" h="100%">
      {props.children}
    </Container>
  );
}

function PageLink(props: { to: string; children: ReactNode }) {
  return (
    <Button
      variant="transparent"
      to={props.to}
      component={ReactRouterNavLink}
      color="white"
    >
      {props.children}
    </Button>
  );
}

function UserMenu() {
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

export default function AppLayout() {
  const me = useMeContext();

  const isAdmin = me.roles.includes("Admin");

  return (
    <AppShell
      styles={{
        header: {
          backgroundColor: "var(--mantine-primary-color-filled)",
        },
        main: {
          backgroundColor: "var(--mantine-color-gray-1)",
        },
      }}
      header={{ height: 72 }}
      padding="xl"
      layout="alt"
    >
      <AppShell.Header>
        <PageContainer>
          <Group h="100%" justify="space-between">
            <Group gap={0}>
              <Title mr="md" c="white">
                Kaizen🔥
              </Title>
              <PageLink to={AppRoutes.Home}>Home</PageLink>
              {isAdmin && (
                <PageLink to={AppRoutes.Exercises}>Manage Exercises</PageLink>
              )}
            </Group>
            <UserMenu />
          </Group>
        </PageContainer>
      </AppShell.Header>

      <AppShell.Main>
        <PageContainer>
          <Outlet />
        </PageContainer>
      </AppShell.Main>
    </AppShell>
  );
}
