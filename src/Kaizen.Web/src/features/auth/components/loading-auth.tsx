import { Center, Loader, Stack, Text } from "@mantine/core";

function LoadingAuth() {
  return (
    <Center m="auto" mih="100vh">
      <Stack mb="xl" gap="xs" align="center">
        <Loader type="bars" size={100} max="auto" />
        <Text ta="center" size="lg">
          Loading App
        </Text>
      </Stack>
    </Center>
  );
}

export default LoadingAuth;
