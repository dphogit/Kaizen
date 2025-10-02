import apiClient from "@/lib/api-client";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  measurementUnitSchema,
  type UpsertWorkout,
  type Workout,
  workoutSchema,
} from "./types";

export const workoutQueryKeys = {
  all: ["workouts"],
  single: (id: Workout["id"]) => [...workoutQueryKeys.all, id] as const,
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

export async function getWorkout(id: Workout["id"]) {
  const response = await apiClient.get(`/workouts/${id}`);
  return workoutSchema.parse(response.data);
}

export function useGetWorkout(id: Workout["id"]) {
  return useQuery({
    queryKey: workoutQueryKeys.single(id),
    queryFn: () => getWorkout(id),
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

export async function recordWorkout(workout: UpsertWorkout) {
  const response = await apiClient.post("/workouts", workout);
  return workoutSchema.parse(response.data);
}

export function useWorkoutMutation() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: recordWorkout,
    onSuccess: (newWorkout) => {
      const updater = (prev: Workout[] | undefined) =>
        prev ? [newWorkout, ...prev] : [newWorkout];

      queryClient.setQueryData(workoutQueryKeys.all, updater);
    },
  });
}

export async function updateWorkout(data: {
  id: Workout["id"];
  workout: UpsertWorkout;
}) {
  const response = await apiClient.put(`/workouts/${data.id}`, data.workout);
  return workoutSchema.parse(response.data);
}

export function useUpdateWorkoutMutation() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: updateWorkout,
    onSuccess: (updatedWorkout) => {
      const updater = (prev: Workout[] | undefined) =>
        prev
          ? prev.map((w) => (w.id === updatedWorkout.id ? updatedWorkout : w))
          : [updatedWorkout];

      queryClient.setQueryData(workoutQueryKeys.all, updater);
    },
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
      const updater = (prev: Workout[] | undefined) =>
        prev ? prev.filter((w) => w.id !== id) : [];
      
      queryClient.setQueryData(workoutQueryKeys.all, updater);
    },
  });
}
