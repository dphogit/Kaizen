import { useExercisesQuery } from "../api";
import { Grid } from "@mantine/core";
import ExerciseCard from "./exercise-card";

export default function ExercisesPage() {
  const query = useExercisesQuery();

  if (query.data) {
    return (
      <Grid>
        {query.data.map((exercise) => (
          <Grid.Col span={4} key={exercise.id}>
            <ExerciseCard exercise={exercise} />
          </Grid.Col>
        ))}
      </Grid>
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
