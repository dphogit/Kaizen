import { Container, Loader, Paper, Text, Title } from "@mantine/core";
import WorkoutForm from "./workout-form";
import { useExercisesQuery } from "@/features/exercises/api";
import { useMeasurementUnits } from "../api";

function Body() {
  const exercisesQuery = useExercisesQuery();
  const unitsQuery = useMeasurementUnits();

  if (exercisesQuery.data && unitsQuery.data) {
    return (
      <WorkoutForm exercises={exercisesQuery.data} units={unitsQuery.data} />
    );
  }

  if (exercisesQuery.error || unitsQuery.error) {
    return <Text>Error setting up form. Try again later.</Text>;
  }

  return <Loader />;
}

export default function RecordWorkoutPage() {
  return (
    <Container maw="900px">
      <Paper>
        <Title mb="md">Record a New Workout</Title>
        <Body />
      </Paper>
    </Container>
  );
}
