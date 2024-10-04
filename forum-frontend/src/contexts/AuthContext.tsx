import {
  createContext,
  useContext,
  useState,
  ReactNode,
  useEffect,
} from "react";
import api from "../utils/api";
import decodeToken from "../utils/tokenDecoder";

type AuthContextType = {
  token: string | null;
  setToken: (token: string | null) => void;
  logout: () => void;
  updateUserActivity: (isActive: boolean) => Promise<void>;
};

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [token, setToken] = useState<string | null>(
    localStorage.getItem("token")
  );

  const logout = async () => {
    await updateUserActivity(false);
    setToken(null);
    localStorage.removeItem("token");

    try {
      await api.get("/auth/logout", {});
    } catch (err) {
      console.error(err);
    }
  };

  const updateUserActivity = async (isActive: boolean) => {
    if (!token) return;

    const [userId] = decodeToken(token);
    try {
      await api.patch(`/users/${userId}/activity`, { isActive });
    } catch (error) {
      console.error("Error updating user activity:", error);
    }
  };

  useEffect(() => {
    if (token) {
      updateUserActivity(true);
    }
  }, [token]);

  return (
    <AuthContext.Provider
      value={{ token, setToken, logout, updateUserActivity }}
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
