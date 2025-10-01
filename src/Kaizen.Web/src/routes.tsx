import { Route, Routes } from "react-router";
import AppLayout from "./components/app-layout";
import { useMeContext } from "./features/auth/stores/me-context";
import ExercisesPage from "./features/exercises/components/exercises-page";
import WorkoutsPage from "./features/workouts/components/workouts-page";
import RecordWorkoutPage from "./features/workouts/components/record-workout-page";

export const AppRoutes = {
  Home: "/",
  Exercises: "exercises",
  Workouts: {
    Index: "/workouts",
    Record: () => `${AppRoutes.Workouts.Index}/record` as const,
  },
} as const;

export function AuthenticatedRoutes() {
  const me = useMeContext();

  const isAdmin = me.roles.includes("Admin");

  return (
    <Routes>
      <Route element={<AppLayout />}>
        <Route index element={<WorkoutsPage />} />
        <Route
          path={AppRoutes.Workouts.Record()}
          element={<RecordWorkoutPage />}
        />
        {isAdmin && (
          <Route path={AppRoutes.Exercises} element={<ExercisesPage />} />
        )}
      </Route>
      {/* TODO Page not found */}
    </Routes>
  );
}
