import { useQuery } from "@tanstack/react-query";
import apiClient from "@/lib/api-client";
import { userSchema } from "../types";
import { AxiosError, HttpStatusCode } from "axios";

export async function getMe() {
  const response = await apiClient.get("/auth");
  return userSchema.parse(response.data);
}

export function useMeQuery() {
  return useQuery({
    queryKey: ["me"],
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
