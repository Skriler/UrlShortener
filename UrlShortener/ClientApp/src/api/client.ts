import axios from "axios";

const API_BASE_URL = "/api";

export const httpClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL + API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

export const getToken = (): string | null => {
  return localStorage.getItem("token");
};

export const setToken = (token: string): void => {
  localStorage.setItem("token", token);
  httpClient.defaults.headers.common["Authorization"] = `Bearer ${token}`;
};

export const removeToken = (): void => {
  localStorage.removeItem("token");
  delete httpClient.defaults.headers.common["Authorization"];
};

const token = getToken();
if (token) {
  httpClient.defaults.headers.common["Authorization"] = `Bearer ${token}`;
}
