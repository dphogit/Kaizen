import type { ReactNode } from "react";
import { createTheme, MantineProvider, Paper } from "@mantine/core";
import { Notifications } from "@mantine/notifications";

export const theme = createTheme({
  defaultRadius: "sm",
  components: {
    Paper: Paper.extend({
      defaultProps: {
        p: "md",
        shadow: "sm",
        radius: "sm",
      },
    }),
  },
});

export function ThemeProvider(props: { children: ReactNode }) {
  return (
    <MantineProvider theme={theme}>
      <Notifications />
      {props.children}
    </MantineProvider>
  );
}
