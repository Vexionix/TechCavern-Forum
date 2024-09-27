import { ReactNode } from "react";
import { useAuth } from "../../contexts/AuthContext";
import { Navigate } from "react-router-dom";
import decodeToken from "../../utils/tokenDecoder";

interface ProtectedRouteProps {
  allowedRoles: string[];
  children: ReactNode;
}

const ForbiddenRedirect = ({ allowedRoles, children }: ProtectedRouteProps) => {
  const { token } = useAuth();
  if (token) {
    const [, role] = decodeToken(token);
    if (!role || !allowedRoles.includes(role)) {
      return <Navigate to="/forbidden" />;
    }
  }

  return <>{children}</>;
};

export default ForbiddenRedirect;
