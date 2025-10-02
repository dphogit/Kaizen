import apiClient from "@/lib/api-client";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  measurementUnitSchema,
  type RecordWorkout,
  type Workout,
  workoutSchema,
} from "./types";

export const workoutQueryKeys = {
  all: ["workouts"],
} as const;

export async function getMyWorkouts() {
  const response = await apiClient.get("/workouts");
  return workoutSchema.array().parse(response.data);
}

export function useMyWorkouts() {
  return useQuery({
    queryKey: workoutQueryKeys.all,
    queryFn: getMyWorkouts,
  });
}

export async function getMeasurementUnits() {
  const response = await apiClient.get("/measurement-units");
  return measurementUnitSchema.array().parse(response.data);
}

export function useMeasurementUnits() {
  return useQuery({
    queryKey: ["measurement-units"],
    queryFn: getMeasurementUnits,

    staleTime: "static",
    gcTime: Infinity,
  });
}

export async function recordWorkout(workout: RecordWorkout) {
  const response = await apiClient.post("/workouts", workout);
  return workoutSchema.parse(response.data);
}

export function useWorkoutMutation() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: recordWorkout,
    onSuccess: () =>
      queryClient.invalidateQueries({
        queryKey: workoutQueryKeys.all,
      }),
  });
}

export async function deleteWorkout(id: Workout["id"]) {
  await apiClient.delete(`/workouts/${id}`);
}

export function useDeleteWorkoutMutation() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: deleteWorkout,
    onSuccess: (_, id) => {
      const updater = (prev: Workout[]) => prev.filter((w) => w.id !== id);
      queryClient.setQueryData(workoutQueryKeys.all, updater);
    },
  });
}
