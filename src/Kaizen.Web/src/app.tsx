import "@mantine/core/styles.css";

import { MantineProvider } from "@mantine/core";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { AuthProvider, useMeContext } from "@/features/auth/stores/me-context";

const queryClient = new QueryClient();

function HomePage() {
  const me = useMeContext();

  return <div>{JSON.stringify(me, null, 2)}</div>;
}

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <MantineProvider>
        <AuthProvider>
          <HomePage />
        </AuthProvider>
      </MantineProvider>
    </QueryClientProvider>
  );
}

export default App;
