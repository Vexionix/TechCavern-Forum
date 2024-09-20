import { ReactNode } from "react";
import { useAuth } from "../../contexts/AuthContext";
import { Navigate } from "react-router-dom";

interface ProtectedRouteProps {
  children: ReactNode;
}

const AuthRedirect = ({ children }: ProtectedRouteProps) => {
  const { token } = useAuth();
  if (token) {
    return <Navigate to="/" />;
  }

  return <>{children}</>;
};

export default AuthRedirect;
