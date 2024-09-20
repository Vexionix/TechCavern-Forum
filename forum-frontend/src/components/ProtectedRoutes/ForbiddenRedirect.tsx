import { ReactNode } from "react";
import { useAuth } from "../../contexts/AuthContext";
import { Navigate } from "react-router-dom";

interface ProtectedRouteProps {
  allowedRoles: string[];
  children: ReactNode;
}

const ForbiddenRedirect = ({ allowedRoles, children }: ProtectedRouteProps) => {
  const { role } = useAuth();
  if (!role || !allowedRoles.includes(role)) {
    return <Navigate to="/forbidden" />;
  }

  return <>{children}</>;
};

export default ForbiddenRedirect;
