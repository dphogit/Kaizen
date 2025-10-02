import { Container, Loader, Paper, Text, Title } from "@mantine/core";
import { useParams } from "react-router";
import { useExercisesQuery } from "@/features/exercises/api";
import { useGetWorkout, useMeasurementUnits } from "../api";
import WorkoutForm from "./workout-form";

export default function EditWorkoutPage() {
  const workoutId = useGetWorkoutId();

  const exercisesQuery = useExercisesQuery();
  const unitsQuery = useMeasurementUnits();
  const workoutQuery = useGetWorkout(workoutId);

  const PageBody =
    exercisesQuery.data && unitsQuery.data && workoutQuery.data ? (
      <WorkoutForm
        exercises={exercisesQuery.data}
        units={unitsQuery.data}
        workout={workoutQuery.data}
      />
    ) : exercisesQuery.error || unitsQuery.error || workoutQuery.error ? (
      <Text>Error setting up form. Try again later.</Text>
    ) : (
      <Loader />
    );

  return (
    <Container maw="900px">
      <Paper>
        <Title mb="md">Edit Workout</Title>
        {PageBody}
      </Paper>
    </Container>
  );
}

function useGetWorkoutId() {
  const params = useParams<{ id: string }>();

  if (!params.id) {
    throw Error("No path param :id");
  }

  const workoutId = parseInt(params.id);

  if (Number.isNaN(workoutId)) {
    // TODO: Probably should handle the error more gracefully (e.g. not found)
    //       This can only happen if user manually types in an invalid ID.
    throw Error("Invalid format for :id");
  }

  return workoutId;
}
