import type { ReactNode } from "react";
import { NavLink as ReactRouterNavLink, Outlet } from "react-router";
import { AppShell, Button, Container, Group, Title } from "@mantine/core";
import { useMeContext } from "@/features/auth/stores/me-context";
import { AppRoutes } from "../routes";
import UserMenu from "./user-menu";

function PageContainer(props: { children: ReactNode }) {
  return (
    <Container maw="900px" px="xl" h="100%">
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
