import {
  Alert,
  Box,
  Button,
  Center,
  Group,
  PasswordInput,
  TextInput,
  Title,
} from "@mantine/core";
import { isEmail, isNotEmpty, useForm } from "@mantine/form";
import { useLoginMutation } from "../api";

import classes from "./login-page.module.css";
import { AxiosError, HttpStatusCode } from "axios";
import { useState } from "react";

function LoginPage() {
  const [loginError, setLoginError] = useState<string | null>(null);

  const form = useForm({
    mode: "uncontrolled",
    initialValues: { email: "", password: "" },
    validate: {
      email: isEmail("Invalid email"),
      password: isNotEmpty("Invalid password"),
    },
  });

  const loginMutation = useLoginMutation();

  function handleSubmit(values: typeof form.values) {
    loginMutation.mutate(values, {
      onError: (error) => {
        if (error instanceof AxiosError) {
          if (error.response?.status === HttpStatusCode.Unauthorized) {
            setLoginError("Incorrect email or password.");
            return;
          }
        }

        setLoginError("There was a problem logging in. Try again later.");
      },
    });
  }

  const isDisabled = loginMutation.isPending;

  return (
    <Center m="auto" mih="100vh">
      <Box w={320}>
        <Title mb="sm">Kaizen🔥</Title>
        {loginError && (
          <Alert mb="sm" variant="light" color="red">
            {loginError}
          </Alert>
        )}
        <form
          className={classes.loginForm}
          onSubmit={form.onSubmit(handleSubmit)}
        >
          <TextInput
            label="Email"
            placeholder="your@email.com"
            key={form.key("email")}
            disabled={isDisabled}
            {...form.getInputProps("email")}
          />
          <PasswordInput
            label="Password"
            placeholder="Enter Password"
            type="password"
            mt="sm"
            key={form.key("password")}
            disabled={isDisabled}
            {...form.getInputProps("password")}
          />
          <Group justify="flex-start" mt="lg">
            <Button type="submit" loading={loginMutation.isPending}>
              Login
            </Button>
          </Group>
        </form>
      </Box>
    </Center>
  );
}

export default LoginPage;
