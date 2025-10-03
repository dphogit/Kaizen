import { Route, Routes } from "react-router";
import AppLayout from "./components/app-layout";
import { useMeContext } from "./features/auth/stores/me-context";
import ExercisesPage from "./features/exercises/components/exercises-page";
import WorkoutsPage from "./features/workouts/components/workouts-page";
import RecordWorkoutPage from "./features/workouts/components/record-workout-page";
import EditWorkoutPage from "./features/workouts/components/edit-workout-page";

export const AppRoutes = {
  Home: "/",
  Exercises: "exercises",
  Workouts: {
    Index: "/workouts",
    Record: () => `${AppRoutes.Workouts.Index}/record` as const,
    Edit: (id?: number | string) =>
      `${AppRoutes.Workouts.Index}/${id ?? ":id"}/edit` as const,
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
        <Route path={AppRoutes.Workouts.Edit()} element={<EditWorkoutPage />} />
        {isAdmin && (
          <Route path={AppRoutes.Exercises} element={<ExercisesPage />} />
        )}
      </Route>
      {/* TODO Page not found */}
    </Routes>
  );
}
