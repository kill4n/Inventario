import axios from "axios";
import type { LoginRequest, RegisterRequest, AuthResponse, RegisterResponse } from "../types/auth.types";

const API_URL = import.meta.env.VITE_API_URL || "http://localhost:5274";

export const api = axios.create({
    baseURL: `${API_URL}/api`,
    headers: {
        "Content-Type": "application/json",
    },
});

// Interceptor para agregar token JWT en cada request
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

// Interceptor para manejar errores de autenticación
api.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response?.status === 401) {
            // Token expirado o inválido
            localStorage.removeItem("token");
            localStorage.removeItem("user");
            window.location.href = "/";
        }
        return Promise.reject(error);
    }
);

export const authAPI = {
    login: async (data: LoginRequest): Promise<AuthResponse> => {
        const response = await api.post<AuthResponse>("/auth/login", data);
        return response.data;
    },

    register: async (data: RegisterRequest): Promise<RegisterResponse> => {
        const response = await api.post<RegisterResponse>("/auth/register", {
            ...data,
            role: data.role || "User"
        });
        return response.data;
    },
};
