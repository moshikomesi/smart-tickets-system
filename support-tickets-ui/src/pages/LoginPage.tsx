import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { authApi } from "../api/authApi";
import { useAuth } from "../auth/AuthContext";

export default function LoginPage() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const { login } = useAuth();
  const navigate = useNavigate();

  async function handleAdminLogin(e: React.FormEvent) {
    e.preventDefault();

    try {
      const { token } = await authApi.login(username, password);
      login(token);
      navigate("/");
    } catch {
      alert("Invalid username or password");
    }
  }

  async function handleGuest() {
    const { token } = await authApi.guest();
    login(token);
    navigate("/");
  }

  return (
    <div className="flex items-center justify-center h-screen bg-gray-100">
      <div className="bg-white p-6 rounded shadow-md w-80">

        <h2 className="text-2xl font-semibold mb-4 text-center">Welcome</h2>

        <form onSubmit={handleAdminLogin}>
          <input
            className="border p-2 w-full mb-3 rounded"
            placeholder="Username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />

          <input
            className="border p-2 w-full mb-3 rounded"
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />

          <button
            type="submit"
            className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700"
          >
            Login as Admin
          </button>
        </form>

        <div className="mt-4 text-center text-gray-500">OR</div>

        <button
          onClick={handleGuest}
          className="w-full mt-3 bg-gray-300 py-2 rounded hover:bg-gray-400"
        >
          Continue as Guest
        </button>
      </div>
    </div>
  );
}
