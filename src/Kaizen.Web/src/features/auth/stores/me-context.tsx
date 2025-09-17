import { createContext, type ReactNode, useContext } from "react";
import { AxiosError, HttpStatusCode } from "axios";

import { useMeQuery } from "../api/get-me";
import type { User } from "../types";
import LoginPage from "../components/login-page";
import LoadingAuth from "../components/loading-auth";

const MeContext = createContext<User | null>(null);

type AuthContextProviderProps = { children: ReactNode };

export function AuthProvider(props: AuthContextProviderProps) {
  const meQuery = useMeQuery();

  if (meQuery.data) {
    return (
      <MeContext.Provider value={meQuery.data}>
        {props.children}
      </MeContext.Provider>
    );
  }

  if (meQuery.isError) {
    if (
      meQuery.error instanceof AxiosError &&
      meQuery.error.status === HttpStatusCode.Unauthorized
    ) {
      return <LoginPage />;
    }

    return <p>Error</p>;
  }

  return <LoadingAuth />;
}

export function useMeContext() {
  const me = useContext(MeContext);

  if (!me) {
    throw new Error("MeContext: No value provided");
  }

  return me;
}
