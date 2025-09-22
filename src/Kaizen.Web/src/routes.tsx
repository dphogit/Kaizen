import { Route, Routes } from "react-router";
import AppLayout from "./components/app-layout";
import { useMeContext } from "./features/auth/stores/me-context";

function HomePage() {
  const me = useMeContext();

  return <div>{JSON.stringify(me, null, 2)}</div>;
}

function ExercisePage() {
  return <p>Exercises Management Page</p>;
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
          <Route path={AppRoutes.Exercises} element={<ExercisePage />} />
        )}
      </Route>
      {/* TODO Page not found */}
    </Routes>
  );
}
