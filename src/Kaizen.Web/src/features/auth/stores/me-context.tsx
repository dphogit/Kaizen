import { createContext, type ReactNode, useContext } from "react";
import { AxiosError, HttpStatusCode } from "axios";

import { useMeQuery } from "../api";
import type { User } from "../types";
import LoginPage from "../components/login-page";
import LoadingAuth from "../components/loading-auth";

const MeContext = createContext<User | null>(null);

type MeContextProviderProps = { children: ReactNode };

export function MeProvider(props: MeContextProviderProps) {
  const meQuery = useMeQuery();

  if (meQuery.isPending) {
    return <LoadingAuth />;
  }

  // Check for error before data, a logout triggers an invalidation on the "me"
  // key which we expect to return 401 because they are now invalidated. This
  // will then correctly show the login page, rather than the authenticated
  // page using stale/invalid data existing in the query cache.
  // https://tkdodo.eu/blog/status-checks-in-react-query
  if (meQuery.isError) {
    if (
      meQuery.error instanceof AxiosError &&
      meQuery.error.status === HttpStatusCode.Unauthorized
    ) {
      return <LoginPage />;
    }

    return <p>Error</p>;
  }

  return (
    <MeContext.Provider value={meQuery.data}>
      {props.children}
    </MeContext.Provider>
  );
}

export function useMeContext() {
  const me = useContext(MeContext);

  if (!me) {
    throw new Error("MeContext: No value provided");
  }

  return me;
}
