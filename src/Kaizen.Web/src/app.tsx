import "@mantine/core/styles.css";
import "@mantine/notifications/styles.css";

import { MantineProvider } from "@mantine/core";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { MeProvider } from "@/features/auth/stores/me-context";
import { Notifications } from "@mantine/notifications";
import { BrowserRouter } from "react-router";
import { AuthenticatedRoutes } from "./routes";

const queryClient = new QueryClient();

function App() {
  return (
    <BrowserRouter>
      <QueryClientProvider client={queryClient}>
        <MantineProvider>
          <Notifications />
          <MeProvider>
            {/* MeProvider's children will only render when authenticated */}
            <AuthenticatedRoutes />
          </MeProvider>
        </MantineProvider>
      </QueryClientProvider>
    </BrowserRouter>
  );
}

export default App;
