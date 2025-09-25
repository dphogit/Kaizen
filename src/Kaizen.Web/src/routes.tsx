import { Route, Routes } from "react-router";
import AppLayout from "./components/app-layout";
import { useMeContext } from "./features/auth/stores/me-context";
import ExercisesPage from "./features/exercises/components/exercises-page";

function HomePage() {
  const me = useMeContext();

  return <div>{JSON.stringify(me, null, 2)}</div>;
}

export const AppRoutes = {
  Home: "/",
  Exercises: "exercises",
} as const;

export function AuthenticatedRoutes() {
  const me = useMeContext();

  const isAdmin = me.roles.includes("Admin");

  return (
    <Routes>
      <Route element={<AppLayout />}>
        <Route index element={<HomePage />} />
        {isAdmin && (
          <Route path={AppRoutes.Exercises} element={<ExercisesPage />} />
        )}
      </Route>
      {/* TODO Page not found */}
    </Routes>
  );
}
