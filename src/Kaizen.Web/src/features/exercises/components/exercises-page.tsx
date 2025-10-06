import { useExercisesQuery } from "../api";
import { Grid } from "@mantine/core";
import ExerciseCard from "./exercise-card";
import type { Exercise } from "../types";
import ExercisesPageHeader from "./exercises-page-header";

function ExercisesGrid(props: { exercises: Exercise[] }) {
  return (
    <Grid>
      {props.exercises.map((exercise) => (
        <Grid.Col span={6} key={exercise.id}>
          <ExerciseCard exercise={exercise} />
        </Grid.Col>
      ))}
    </Grid>
  );
}

export default function ExercisesPage() {
  const query = useExercisesQuery();

  if (query.data) {
    return (
      <>
        <ExercisesPageHeader />
        <ExercisesGrid exercises={query.data} />
      </>
    );
  }

  // TODO: Error State
  if (query.error) {
    console.error(query.error);
    return <p>Error fetching exercises.</p>;
  }

  // TODO: Loading State
  return <p>Loading...</p>;
}
