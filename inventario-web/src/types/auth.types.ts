export interface LoginRequest {
    username: string;
    password: string;
}

export interface RegisterRequest {
    username: string;
    password: string;
    email?: string;
    role?: string;
}

export interface AuthResponse {
    token: string;
    userId: string;
    username: string;
    email: string;
    role: string;
}

export interface RegisterResponse {
    userId: string;
    username: string;
    email: string;
    role: string;
}

export interface User {
    userId: string;
    username: string;
    email: string;
    role: string;
}
