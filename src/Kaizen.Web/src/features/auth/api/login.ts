import { z } from "zod";
import apiClient from "@/lib/api-client";
import { useMutation } from "@tanstack/react-query";

const loginSchema = z.object({
  email: z.email(),
  password: z.string(),
});

export type Login = z.infer<typeof loginSchema>;

export async function login(loginData: Login) {
  await apiClient.post("/auth/login?useSessionCookies=true", loginData);
}

export function useLoginMutation() {
  return useMutation({
    mutationFn: login,
  });
}
