import { createBrowserRouter, RouterProvider } from "react-router-dom";
import HomePage from "./components/HomePage/HomePage.tsx";
import LoginPage from "./components/LoginPage/LoginPage.tsx";
import RegisterPage from "./components/RegisterPage/RegisterPage.tsx";
import Users from "./components/Users.tsx";
import NotFound from "./components/NotFound/NotFound.tsx";

import "./App.css";
import { AuthProvider } from "./contexts/AuthContext.tsx";
import AuthRedirect from "./components/ProtectedRoutes/AuthRedirect.tsx";

const router = createBrowserRouter([
  {
    path: "/",
    element: <HomePage />,
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
    element: <Users />,
  },
  {
    path: "*",
    element: <NotFound />,
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
