import axios from "axios";

const API_URL = import.meta.env.VITE_API_URL || "http://localhost:5274";

const api = axios.create({
    baseURL: `${API_URL}/api`,
    headers: {
        "Content-Type": "application/json",
    },
});

api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem("token");
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)

);
export const authAPI = {
    login: async (username: string, password: string) => {
        const response = await api.post("/auth/login", { username, password });
        return response.data;
    },

    register: async (username: string, password: string, email?: string) => {
        const response = await api.post("/auth/register", {
            username,
            password,
            email,
            role: "User"
        });
        return response.data;
    },
};