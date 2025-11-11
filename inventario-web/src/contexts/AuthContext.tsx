import { createContext, useContext, useState, useEffect, ReactNode } from "react";
import type { User, LoginRequest, RegisterRequest, AuthResponse } from "../types/auth.types";
import { authAPI } from "../services/api";

interface AuthContextType {
    user: User | null;
    isAuthenticated: boolean;
    isLoading: boolean;
    login: (data: LoginRequest) => Promise<void>;
    register: (data: RegisterRequest) => Promise<void>;
    logout: () => void;
    error: string | null;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error("useAuth must be used within an AuthProvider");
    }
    return context;
};

interface AuthProviderProps {
    children: ReactNode;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
    const [user, setUser] = useState<User | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    // Cargar usuario desde localStorage al iniciar
    useEffect(() => {
        const storedUser = localStorage.getItem("user");
        const storedToken = localStorage.getItem("token");

        if (storedUser && storedToken) {
            setUser(JSON.parse(storedUser));
        }
        setIsLoading(false);
    }, []);

    const login = async (data: LoginRequest) => {
        try {
            setIsLoading(true);
            setError(null);

            const response: AuthResponse = await authAPI.login(data);

            // Guardar token y usuario
            localStorage.setItem("token", response.token);
            localStorage.setItem("user", JSON.stringify({
                userId: response.userId,
                username: response.username,
                email: response.email,
                role: response.role,
            }));

            setUser({
                userId: response.userId,
                username: response.username,
                email: response.email,
                role: response.role,
            });
        } catch (err: any) {
            const errorMessage = err.response?.data?.message || "Error al iniciar sesión";
            setError(errorMessage);
            throw new Error(errorMessage);
        } finally {
            setIsLoading(false);
        }
    };

    const register = async (data: RegisterRequest) => {
        try {
            setIsLoading(true);
            setError(null);

            const response = await authAPI.register(data);

            // Después de registrarse, hacer login automáticamente
            await login({ username: data.username, password: data.password });
        } catch (err: any) {
            const errorMessage = err.response?.data?.message || "Error al registrarse";
            setError(errorMessage);
            throw new Error(errorMessage);
        } finally {
            setIsLoading(false);
        }
    };

    const logout = () => {
        localStorage.removeItem("token");
        localStorage.removeItem("user");
        setUser(null);
        setError(null);
    };

    const value: AuthContextType = {
        user,
        isAuthenticated: !!user,
        isLoading,
        login,
        register,
        logout,
        error,
    };

    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};
