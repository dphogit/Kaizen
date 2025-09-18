import Axios from "axios";

// TODO: Inject from configuration
const BASE_URL = "https://localhost:8081";

const apiClient = Axios.create({
  baseURL: BASE_URL,
  withCredentials: true,
});

export default apiClient;
