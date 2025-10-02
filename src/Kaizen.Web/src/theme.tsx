import type { ReactNode } from "react";
import {
  ActionIcon, Alert,
  createTheme,
  MantineProvider,
  Modal,
  Paper
} from "@mantine/core";
import { Notifications } from "@mantine/notifications";

export const theme = createTheme({
  defaultRadius: "sm",
  components: {
    ActionIcon: ActionIcon.extend({
      defaultProps: {
        variant: "subtle",
        color: "gray",
        radius: "sm",
      },
    }),
    Alert: Alert.extend({
      defaultProps: {
        variant: "light"
      }
    }),
    Modal: Modal.extend({
      defaultProps: {
        size: "lg",
        styles: {
          title: {
            fontSize: "var(--mantine-font-size-xl)",
          },
        },
      },
    }),
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
