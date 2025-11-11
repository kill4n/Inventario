import { useState } from "react";
import { useAuth } from "../contexts/AuthContext";
import "./Header.css";

export const Header = () => {
    const { user, isAuthenticated, login, logout, error } = useAuth();
    const [showLogin, setShowLogin] = useState(false);
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [localError, setLocalError] = useState<string | null>(null);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLocalError(null);

        if (!username || !password) {
            setLocalError("Por favor completa todos los campos");
            return;
        }

        try {
            setIsSubmitting(true);
            await login({ username, password });
            // Limpiar formulario y cerrar modal
            setUsername("");
            setPassword("");
            setShowLogin(false);
        } catch (err: any) {
            setLocalError(err.message || "Error al iniciar sesión");
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleLogout = () => {
        logout();
        setShowLogin(false);
    };

    return (
        <header className="header">
            <div className="header-container">
                <div className="header-logo">
                    <h1>Inventario App</h1>
                </div>

                <nav className="header-nav">
                    {isAuthenticated ? (
                        <div className="user-menu">
                            <span className="user-greeting">
                                Hola, <strong>{user?.username}</strong>
                            </span>
                            <span className="user-role">({user?.role})</span>
                            <button onClick={handleLogout} className="btn btn-secondary">
                                Cerrar Sesión
                            </button>
                        </div>
                    ) : (
                        <button onClick={() => setShowLogin(!showLogin)} className="btn btn-primary">
                            Iniciar Sesión
                        </button>
                    )}
                </nav>
            </div>

            {/* Modal de Login */}
            {showLogin && !isAuthenticated && (
                <div className="login-modal">
                    <div className="login-modal-backdrop" onClick={() => setShowLogin(false)} />
                    <div className="login-modal-content">
                        <div className="login-modal-header">
                            <h2>Iniciar Sesión</h2>
                            <button
                                className="close-button"
                                onClick={() => setShowLogin(false)}
                                aria-label="Cerrar"
                            >
                                ×
                            </button>
                        </div>

                        <form onSubmit={handleSubmit} className="login-form">
                            {(localError || error) && (
                                <div className="error-message">
                                    {localError || error}
                                </div>
                            )}

                            <div className="form-group">
                                <label htmlFor="username">Usuario</label>
                                <input
                                    type="text"
                                    id="username"
                                    value={username}
                                    onChange={(e) => setUsername(e.target.value)}
                                    placeholder="Ingresa tu usuario"
                                    disabled={isSubmitting}
                                    autoFocus
                                />
                            </div>

                            <div className="form-group">
                                <label htmlFor="password">Contraseña</label>
                                <input
                                    type="password"
                                    id="password"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    placeholder="Ingresa tu contraseña"
                                    disabled={isSubmitting}
                                />
                            </div>

                            <button
                                type="submit"
                                className="btn btn-primary btn-block"
                                disabled={isSubmitting}
                            >
                                {isSubmitting ? "Ingresando..." : "Ingresar"}
                            </button>
                        </form>

                        <div className="login-footer">
                            <p>¿No tienes cuenta? <a href="/register">Registrarse</a></p>
                        </div>
                    </div>
                </div>
            )}
        </header>
    );
};
