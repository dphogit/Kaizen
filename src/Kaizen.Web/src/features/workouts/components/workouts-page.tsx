import { Button, Center, Group, Loader, Stack, Title } from "@mantine/core";
import { useMyWorkouts } from "../api";
import WorkoutCard from "./workout-card";
import { Link } from "react-router";
import { AppRoutes } from "@/routes";
import { IconPlus } from "@tabler/icons-react";

export default function WorkoutsPage() {
  const workoutsQuery = useMyWorkouts();

  if (workoutsQuery.data) {
    return (
      <>
        <Group justify="space-between">
          <Title>Workout Activity</Title>
          <Button
            component={Link}
            to={AppRoutes.Workouts.Record()}
            leftSection={<IconPlus size={18} />}
          >
            Record Workout
          </Button>
        </Group>
        <Stack justify="flex-start" mt="xl">
          {workoutsQuery.data.map((workout) => (
            <WorkoutCard key={workout.id} workout={workout} />
          ))}
        </Stack>
      </>
    );
  }

  if (workoutsQuery.isPending) {
    return (
      <Center>
        <Loader />
      </Center>
    );
  }

  if (workoutsQuery.error) {
    return <p>Error loading workouts.</p>;
  }
}
