import { createBrowserRouter, RouterProvider } from "react-router-dom";
import HomePage from "./pages/HomePage/HomePage.tsx";
import LoginPage from "./pages/LoginPage/LoginPage.tsx";
import RegisterPage from "./pages/RegisterPage/RegisterPage.tsx";
import Users from "./pages/UsersPage/Users.tsx";
import NotFoundPage from "./pages/NotFoundPage/NotFoundPage.tsx";
import ForbiddenPage from "./pages/ForbiddenPage/ForbiddenPage.tsx";

import "./App.css";
import { AuthProvider } from "./contexts/AuthContext.tsx";
import AuthRedirect from "./pages/ProtectedRoutes/AuthRedirect.tsx";
import ForbiddenRedirect from "./pages/ProtectedRoutes/ForbiddenRedirect.tsx";
import NotLoggedInRedirect from "./pages/ProtectedRoutes/NotLoggedInRedirect.tsx";

const router = createBrowserRouter([
  {
    path: "/",
    element: (
      <NotLoggedInRedirect>
        <HomePage />
      </NotLoggedInRedirect>
    ),
  },
  {
    path: "/login",
    element: (
      <AuthRedirect>
        <LoginPage />
      </AuthRedirect>
    ),
  },
  {
    path: "/register",
    element: (
      <AuthRedirect>
        <RegisterPage />
      </AuthRedirect>
    ),
  },
  {
    path: "/users",
    element: (
      <NotLoggedInRedirect>
        <ForbiddenRedirect allowedRoles={["Admin"]}>
          <Users />
        </ForbiddenRedirect>
      </NotLoggedInRedirect>
    ),
  },
  {
    path: "*",
    element: (
      <NotLoggedInRedirect>
        <NotFoundPage />
      </NotLoggedInRedirect>
    ),
  },
  {
    path: "/forbidden",
    element: (
      <NotLoggedInRedirect>
        <ForbiddenPage />
      </NotLoggedInRedirect>
    ),
  },
]);

function App() {
  return (
    <AuthProvider>
      <RouterProvider router={router} />
    </AuthProvider>
  );
}

export default App;
