import { z } from "zod";

export const muscleGroupSchema = z.object({
  code: z.string(),
  name: z.string(),
});

export const createExerciseSchema = z.object({
  name: z.string(),
  muscleGroupCodes: z.string().array(),
});

export const exerciseSchema = z.object({
  id: z.int(),
  name: z.string(),
  muscleGroups: z.array(muscleGroupSchema),
});

export type MuscleGroup = z.infer<typeof muscleGroupSchema>;
export type CreateExercise = z.infer<typeof createExerciseSchema>;
export type Exercise = z.infer<typeof exerciseSchema>;
