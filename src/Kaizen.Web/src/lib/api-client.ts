import Axios, { type AxiosError } from "axios";
import { z } from "zod";

// TODO: Set via app configuration.
const BASE_URL = "https://localhost:8081";

const apiClient = Axios.create({
  baseURL: BASE_URL,
  withCredentials: true,
});

const problemDetailsSchema = z.object({
  type: z.string(),
  title: z.string(),
  status: z.number(),
  detail: z.string(),
  errors: z.string().array(),
});

// Extracts the array of validation errors from the problem details response.
// Caller must check for 400 before calling this.
export function getValidationErrors(error: AxiosError): string[] {
  if (!error.response) {
    throw new Error("No response object in AxiosError");
  }

  const problemDetails = problemDetailsSchema.parse(error.response.data);

  return problemDetails.errors.map((errMessage) => errMessage);
}

export default apiClient;
