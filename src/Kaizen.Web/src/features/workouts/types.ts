import { z } from "zod";
import { datetimeSchema } from "@/lib/dates";

export const upsertWorkoutSchema = z.object({
  name: z.string(),
  performedAt: datetimeSchema,
  sets: z
    .object({
      exerciseId: z.int(),
      repetitions: z.int(),
      quantity: z.number(),
      measurementUnitCode: z.string(),
    })
    .array(),
});

export const workoutSchema = z.object({
  id: z.int(),
  name: z.string(),
  performedAt: datetimeSchema,
  sets: z
    .object({
      id: z.number(),
      exerciseId: z.int(),
      exerciseName: z.string(),
      repetitions: z.int(),
      quantity: z.number(),
      measurementUnitCode: z.string(),
    })
    .array(),
});

export type UpsertWorkout = z.infer<typeof upsertWorkoutSchema>;
export type Workout = z.infer<typeof workoutSchema>;

export const measurementUnitSchema = z.object({
  code: z.string(),
  name: z.string(),
});

export type MeasurementUnit = z.infer<typeof measurementUnitSchema>;
