import "@mantine/core/styles.css";
import "@mantine/notifications/styles.css";

import { MantineProvider } from "@mantine/core";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { MeProvider, useMeContext } from "@/features/auth/stores/me-context";
import AppLayout from "@/components/app-layout";
import type { ReactNode } from "react";
import { Notifications } from "@mantine/notifications";

const queryClient = new QueryClient();

function HomePage() {
  const me = useMeContext();

  return <div>{JSON.stringify(me, null, 2)}</div>;
}

function AppProvider(props: { children: ReactNode }) {
  return (
    <QueryClientProvider client={queryClient}>
      <MantineProvider>
        <Notifications />
        <MeProvider>{props.children}</MeProvider>
      </MantineProvider>
    </QueryClientProvider>
  );
}

function App() {
  return (
    <AppProvider>
      <AppLayout>
        <HomePage />
      </AppLayout>
    </AppProvider>
  );
}

export default App;
