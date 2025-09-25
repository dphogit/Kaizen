import "@mantine/core/styles.css";
import "@mantine/notifications/styles.css";

import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { MeProvider } from "@/features/auth/stores/me-context";
import { BrowserRouter } from "react-router";
import { AuthenticatedRoutes } from "./routes";
import { ThemeProvider } from "./theme";

const queryClient = new QueryClient();

function App() {
  return (
    <BrowserRouter>
      <QueryClientProvider client={queryClient}>
        <ThemeProvider>
          <MeProvider>
            {/* MeProvider's children will only render when authenticated */}
            <AuthenticatedRoutes />
          </MeProvider>
        </ThemeProvider>
      </QueryClientProvider>
    </BrowserRouter>
  );
}

export default App;
