import apiClient from "../../lib/api-client";
import { type Login, userSchema } from "./types";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { AxiosError, HttpStatusCode } from "axios";

export const authQueryKeys = {
  me: ["me"] as const,
} as const;

export async function getMe() {
  const response = await apiClient.get("/auth");
  return userSchema.parse(response.data);
}

export function useMeQuery() {
  return useQuery({
    queryKey: authQueryKeys.me,
    queryFn: getMe,
    refetchOnWindowFocus: false, // We only use this query for the initial load
    retry: (_, error) => {
      const notAuthenticated =
        error instanceof AxiosError &&
        error.status == HttpStatusCode.Unauthorized;

      // Don't retry if we know initial auth failed. Saves network requests.
      return !notAuthenticated;
    },
  });
}

export async function login(loginData: Login) {
  await apiClient.post("/auth/login?useSessionCookies=true", loginData);
}

export function useLoginMutation() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: login,
    onSuccess: () =>
      queryClient.fetchQuery({
        queryKey: authQueryKeys.me,
        queryFn: getMe,
      }),
  });
}

export function logout() {
  return apiClient.post("/auth/logout");
}

export function useLogoutMutation() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: logout,
    onSuccess: () =>
      // Refetching once logged out will re-render to the login screen
      queryClient.invalidateQueries({
        queryKey: authQueryKeys.me,
      }),
  });
}
