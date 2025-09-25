import apiClient from "@/lib/api-client";
import {
  type UpsertExercise,
  exerciseSchema,
  muscleGroupSchema,
  type Exercise,
} from "./types";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

export const exerciseQueryKeys = {
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

export async function createExercise(exercise: UpsertExercise) {
  const response = await apiClient.post("/exercises", exercise);
  return exerciseSchema.parse(response.data);
}

export function useExerciseMutation() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: createExercise,
    onSuccess: (newExercise) => {
      const updater = (prev: Exercise[]) => [...prev, newExercise];
      queryClient.setQueryData(exerciseQueryKeys.all, updater);
    },
  });
}

async function editExercise(req: { id: number; payload: UpsertExercise }) {
  const response = await apiClient.put(`/exercises/${req.id}`, req.payload);
  return exerciseSchema.parse(response.data);
}

export function useEditExerciseMutation() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: editExercise,
    onSuccess: (updatedExercise) => {
      const updater = (prev: Exercise[]) =>
        prev.map((e) => (e.id === updatedExercise.id ? updatedExercise : e));

      queryClient.setQueryData(exerciseQueryKeys.all, updater);
    },
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
