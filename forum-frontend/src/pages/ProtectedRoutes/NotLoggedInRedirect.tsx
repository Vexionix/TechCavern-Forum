import { ReactNode } from "react";
import { useAuth } from "../../contexts/AuthContext";
import { Navigate } from "react-router-dom";

interface ProtectedRouteProps {
  children: ReactNode;
}

const NotLoggedInRedirect = ({ children }: ProtectedRouteProps) => {
  const { token } = useAuth();
  if (!token) {
    return <Navigate to="/login" />;
  }

  return <>{children}</>;
};

export default NotLoggedInRedirect;
