import apiClient from "@/lib/api-client";
import { exerciseSchema } from "./types";
import { useQuery } from "@tanstack/react-query";

const exerciseQueryKeys = {
  all: ["exercises"],
} as const;

async function getExercises() {
  const response = await apiClient.get("/exercises");
  return exerciseSchema.array().parse(response.data);
}

export function useExercisesQuery() {
  return useQuery({
    queryKey: exerciseQueryKeys.all,
    queryFn: getExercises,
  });
}
