import {
  QueryClient,
  QueryClientProvider,
  useQuery,
} from "@tanstack/react-query";

const queryClient = new QueryClient();

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <HelloWorld />
    </QueryClientProvider>
  );
}

function HelloWorld() {
  const { isPending, error, data } = useQuery({
    queryKey: ["hello"],
    queryFn: () =>
      fetch("https://localhost:8081").then((response) => response.text()),
  });

  if (isPending) return "Loading...";

  if (error) return `An error occurred: ${error.message}`;

  return data;
}

export default App;
