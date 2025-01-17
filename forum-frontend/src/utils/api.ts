import axios from "axios";

const api = axios.create({
  baseURL: "https://localhost:7072/api",
  withCredentials: true,
});

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers["Authorization"] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

api.interceptors.response.use(
  (response) => {
    return response;
  },
  async (error) => {
    if (error.code != "ERR_CANCELED" && error.name != "CanceledError") {
      if (error.response.status === 403) {
        window.location.href = "/forbidden";
        return Promise.reject(error);
      }

      const originalRequest = error.config;

      if (error.response.status === 401 && !originalRequest._retry) {
        originalRequest._retry = true;

        try {
          const response = await api.get("auth/refresh", {});

          localStorage.setItem("token", response.data.token);

          api.defaults.headers[
            "Authorization"
          ] = `Bearer ${response.data.token}`;
          originalRequest.headers[
            "Authorization"
          ] = `Bearer ${response.data.token}`;

          return api(originalRequest);
        } catch (error) {
          console.error("Token refresh failed");
          localStorage.removeItem("token");
          localStorage.removeItem("userId");
          localStorage.removeItem("role");
          window.location.href = "/login";
        }
      }
    }
    return Promise.reject(error);
  }
);

export default api;
