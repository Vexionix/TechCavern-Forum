import React, { useRef, useState, useEffect, MutableRefObject } from "react";
import "./LoginPage.css";
import { FaUser, FaLock } from "react-icons/fa";
import { Link } from "react-router-dom";
import api from "../../utils/api";
import axios from "axios";
import { jwtDecode, JwtPayload } from "jwt-decode";
import { useAuth } from "../../contexts/AuthContext";

interface DecodedToken {
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": string;
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string;
  exp: number;
}

const LoginPage: React.FC = () => {
  const userRef = useRef() as MutableRefObject<HTMLInputElement>;

  const { setToken, setUserId, setRole } = useAuth();
  const [username, setUsername] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [error, setError] = useState<string>("");
  const [success, setSuccess] = useState<boolean>(false);

  useEffect(() => {
    userRef.current.focus();
  }, []);

  useEffect(() => {
    setError("");
  }, [username, password]);

  const handleLogin = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    try {
      const res = await api.post("/auth/login", {
        Username: username,
        Password: password,
      });

      const accessToken = res?.data?.token;

      if (accessToken) {
        const decodedToken: DecodedToken = jwtDecode<DecodedToken>(accessToken);
        const userId =
          decodedToken[
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
          ];
        const role =
          decodedToken[
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
          ];

        localStorage.setItem("token", accessToken);
        localStorage.setItem("userId", userId);
        localStorage.setItem("role", role);

        setToken(accessToken);
        setUserId(userId);
        setRole(role);
      }

      setUsername("");
      setPassword("");
      setSuccess(true);
    } catch (err) {
      if (axios.isAxiosError(err)) {
        if (!err?.response) {
          setError("No Server Response");
        } else if (err.response?.status === 400) {
          setError(
            "Incorrect username or password. Check your input and try again."
          );
        } else {
          setError("Login Failed");
        }
        setSuccess(false);
      } else console.error(err);
    }
  };

  return (
    <div className="login-body">
      {success ? (
        <div className="wrapper-success">
          <h1>Success!</h1>
          <Link to="/" className="btn btn-primary">
            Go to home page
          </Link>
        </div>
      ) : (
        <div className="wrapper">
          {error && (
            <p className="errMsg" aria-live="assertive">
              {error}
            </p>
          )}
          <h1>Login</h1>
          <form onSubmit={handleLogin}>
            <div className="input-box">
              <input
                type="text"
                id="username"
                ref={userRef}
                placeholder="Username"
                autoComplete="off"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                required
              />
              <FaUser className="icon" />
            </div>
            <div className="input-box">
              <input
                type="password"
                id="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
              <FaLock className="icon" />
            </div>
            <div className="forgot-password">
              <Link to="/recovery">Forgot password?</Link>
            </div>
            <button type="submit">Login</button>
            <div className="register-link">
              <p>
                Don't have an account? <Link to="/register">Register</Link>
              </p>
            </div>
          </form>
        </div>
      )}
    </div>
  );
};

export default LoginPage;
