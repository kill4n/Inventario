import { AuthProvider } from "./contexts/AuthContext";
import { Header } from "./components/Header";
import "./App.css";

function App() {
    return (
        <AuthProvider>
            <div className="app">
                <Header />
                <main className="main-content">
                    <div className="container">
                        <h2>Bienvenido al Sistema de Inventario</h2>
                        <p>Inicia sesión para comenzar a gestionar tu inventario.</p>
                    </div>
                </main>
            </div>
        </AuthProvider>
    );
}

export default App;
