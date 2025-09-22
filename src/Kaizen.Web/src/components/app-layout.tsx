import {
  AppShell,
  Button,
  Group,
  Menu,
  NavLink as MantineNavLink,
  Stack,
  Text,
  Title,
  useMantineTheme,
} from "@mantine/core";
import {
  IconBarbell,
  IconChevronDown,
  IconHome,
  IconLogout,
  IconUser,
} from "@tabler/icons-react";
import { useMeContext } from "@/features/auth/stores/me-context";
import { useLogoutMutation } from "@/features/auth/api";
import { notifications } from "@mantine/notifications";
import { NavLink as ReactRouterNavLink, Outlet } from "react-router";
import { AppRoutes } from "../routes";
import * as React from "react";

const HEADER_HEIGHT = 60;
const SIDENAV_WIDTH = 270;

export default function AppLayout() {
  const me = useMeContext();
  const theme = useMantineTheme();

  const isAdmin = me.roles.includes("Admin");

  return (
    <AppShell
      header={{ height: HEADER_HEIGHT }}
      navbar={{ width: SIDENAV_WIDTH, breakpoint: "xs" }}
      padding="xl"
      layout="alt"
    >
      <AppShell.Header>
        <Group h="100%" px="xl" justify="flex-end">
          <UserMenu />
        </Group>
      </AppShell.Header>

      <AppShell.Navbar>
        <AppShell.Section
          h={HEADER_HEIGHT}
          style={{
            borderBottomWidth: "1px",
            borderBottomStyle: "solid",
            borderBottomColor: theme.colors.gray[3],
          }}
        >
          <Stack h="100%" align="flex-start" justify="center" px="md">
            <Title>Kaizen🔥</Title>
          </Stack>
        </AppShell.Section>

        <AppShell.Section>
          <NavItem
            to={AppRoutes.Home}
            label="Home"
            leftSection={<IconHome size={20} stroke={2} />}
          />
          {isAdmin && <AdminNavigation />}
        </AppShell.Section>
      </AppShell.Navbar>

      <AppShell.Main>
        <Outlet />
      </AppShell.Main>
    </AppShell>
  );
}

function AdminNavigation() {
  return (
    <NavItem
      to={AppRoutes.Exercises}
      label="Manage Exercises"
      leftSection={<IconBarbell size={20} stroke={2} />}
    />
  );
}

type NavItemProps = {
  to: string;
  label: React.ReactNode;
  leftSection?: React.ReactNode;
};

function NavItem(props: NavItemProps) {
  return (
    <MantineNavLink
      component={ReactRouterNavLink}
      to={props.to}
      label={<Text pl={4}>{props.label}</Text>}
      leftSection={props.leftSection}
      py="sm"
      px="md"
    />
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
          variant="default"
          rightSection={<IconChevronDown size={14} />}
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
