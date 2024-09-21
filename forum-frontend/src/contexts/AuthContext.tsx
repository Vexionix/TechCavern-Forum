import { createContext, useContext, useState, ReactNode } from "react";
import api from "../utils/api";

type AuthContextType = {
  token: string | null;
  userId: string | null;
  role: string | null;
  setToken: (token: string | null) => void;
  setUserId: (userId: string | null) => void;
  setRole: (role: string | null) => void;
  logout: () => void;
};

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [token, setToken] = useState<string | null>(
    localStorage.getItem("token")
  );
  const [userId, setUserId] = useState<string | null>(
    localStorage.getItem("userId")
  );
  const [role, setRole] = useState<string | null>(localStorage.getItem("role"));

  const logout = async () => {
    setToken(null);
    setUserId(null);
    setRole(null);
    localStorage.removeItem("token");
    localStorage.removeItem("userId");
    localStorage.removeItem("role");

    try {
      await api.get("/auth/logout", {});
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <AuthContext.Provider
      value={{ token, userId, role, setToken, setUserId, setRole, logout }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
