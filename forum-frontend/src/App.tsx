import { createBrowserRouter, RouterProvider } from "react-router-dom";
import HomePage from "./pages/HomePage/HomePage.tsx";
import LoginPage from "./pages/LoginPage/LoginPage.tsx";
import RegisterPage from "./pages/RegisterPage/RegisterPage.tsx";
import NotFoundPage from "./pages/NotFoundPage/NotFoundPage.tsx";
import ForbiddenPage from "./pages/ForbiddenPage/ForbiddenPage.tsx";
import RulesPage from "./pages/RulesPage/RulesPage.tsx";
import FAQPage from "./pages/FAQPage/FAQPage.tsx";

import Header from "./components/Header/Header.tsx";
import Footer from "./components/Footer/Footer.tsx";

import "./App.css";
import { AuthProvider, useAuth } from "./contexts/AuthContext.tsx";
import AuthRedirect from "./pages/ProtectedRoutes/AuthRedirect.tsx";
import ForbiddenRedirect from "./pages/ProtectedRoutes/ForbiddenRedirect.tsx";
import NotLoggedInRedirect from "./pages/ProtectedRoutes/NotLoggedInRedirect.tsx";
import ContactPage from "./pages/ContactPage/ContactPage.tsx";
import { useEffect, useState } from "react";
import SubcategoryPostsPage from "./pages/SubcategoryPostsPage/SubcategoryPostsPage.tsx";
import AddPostPage from "./pages/AddPostPage/AddPostPage.tsx";
import PostPage from "./pages/PostPage/PostPage.tsx";
import EditCommentPage from "./pages/EditCommentPage/EditCommentPage.tsx";
import EditPostPage from "./pages/EditPostPage/EditPostPage.tsx";
import ScrollToTop from "./utils/ScrollToTop.tsx";
import StaffPage from "./pages/StaffPage/StaffPage.tsx";
import UserProfilePage from "./pages/UserProfilePage/UserProfilePage.tsx";
import EditProfilePage from "./pages/EditProfilePage/EditProfilePage.tsx";

const router = createBrowserRouter([
  {
    path: "/",
    element: (
      <NotLoggedInRedirect>
        <Header />
        <HomePage />
        <Footer />
      </NotLoggedInRedirect>
    ),
  },
  {
    path: "/rules",
    element: (
      <NotLoggedInRedirect>
        <Header />
        <RulesPage />
        <Footer />
      </NotLoggedInRedirect>
    ),
  },
  {
    path: "/faq",
    element: (
      <NotLoggedInRedirect>
        <Header />
        <FAQPage />
        <Footer />
      </NotLoggedInRedirect>
    ),
  },
  {
    path: "/contact",
    element: (
      <NotLoggedInRedirect>
        <Header />
        <ContactPage />
        <Footer />
      </NotLoggedInRedirect>
    ),
  },
  {
    path: "/staff",
    element: (
      <NotLoggedInRedirect>
        <Header />
        <StaffPage />
        <Footer />
      </NotLoggedInRedirect>
    ),
  },
  {
    path: "/subcategory/:id",
    element: (
      <NotLoggedInRedirect>
        <ScrollToTop />
        <Header />
        <SubcategoryPostsPage />
        <Footer />
      </NotLoggedInRedirect>
    ),
  },
  {
    path: "/add-post/:subcategoryId",
    element: (
      <NotLoggedInRedirect>
        <AddPostPage />
      </NotLoggedInRedirect>
    ),
  },
  {
    path: "/edit-profile/:userId",
    element: (
      <NotLoggedInRedirect>
        <EditProfilePage />
      </NotLoggedInRedirect>
    ),
  },
  {
    path: "/edit-comment/:commentId",
    element: (
      <NotLoggedInRedirect>
        <EditCommentPage />
      </NotLoggedInRedirect>
    ),
  },
  {
    path: "/edit-post/:postId",
    element: (
      <NotLoggedInRedirect>
        <EditPostPage />
      </NotLoggedInRedirect>
    ),
  },
  {
    path: "/post/:postId",
    element: (
      <NotLoggedInRedirect>
        <ScrollToTop />
        <Header />
        <PostPage />
        <Footer />
      </NotLoggedInRedirect>
    ),
  },
  {
    path: "/user/profile/:userId",
    element: (
      <NotLoggedInRedirect>
        <ScrollToTop />
        <Header />
        <UserProfilePage />
        <Footer />
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
      <MainApp />
    </AuthProvider>
  );
}

const MainApp = () => {
  const { token, updateUserActivity } = useAuth();
  const [isActive, setIsActive] = useState(false);

  useEffect(() => {
    const handleVisibilityChange = () => {
      if (token) {
        const newIsActive = document.visibilityState === "visible";
        if (newIsActive !== isActive) {
          setIsActive(newIsActive);
          updateUserActivity(newIsActive);
        }
      }
    };

    const handleBeforeUnload = () => {
      if (token) {
        updateUserActivity(false);
      }
    };

    document.addEventListener("visibilitychange", handleVisibilityChange);
    window.addEventListener("beforeunload", handleBeforeUnload);

    if (token && document.visibilityState === "visible") {
      updateUserActivity(true);
      setIsActive(true);
    }

    return () => {
      document.removeEventListener("visibilitychange", handleVisibilityChange);
      window.removeEventListener("beforeunload", handleBeforeUnload);
    };
  }, [token, isActive, updateUserActivity]);

  return <RouterProvider router={router} />;
};

export default App;
