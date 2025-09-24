import apiClient from "@/lib/api-client";
import {
  type CreateExercise,
  exerciseSchema,
  muscleGroupSchema,
} from "./types";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

const exerciseQueryKeys = {
  all: ["exercises"],
  muscleGroups: ["muscle-groups"],
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

export async function createExercise(exercise: CreateExercise) {
  const response = await apiClient.post("/exercises", exercise);
  return exerciseSchema.parse(response.data);
}

export function useExerciseMutation() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: createExercise,
    onSuccess: () =>
      queryClient.invalidateQueries({
        queryKey: exerciseQueryKeys.all,
      }),
  });
}

async function getMuscleGroups() {
  const response = await apiClient.get("/muscle-groups");
  return muscleGroupSchema.array().parse(response.data);
}

export function useMuscleGroupsQuery() {
  return useQuery({
    queryKey: exerciseQueryKeys.muscleGroups,
    queryFn: getMuscleGroups,

    // Muscle groups data is static and does not change during app runtime.
    staleTime: Infinity,
    gcTime: Infinity,
    refetchOnWindowFocus: false,
    refetchOnReconnect: false,
    refetchOnMount: false,
  });
}
