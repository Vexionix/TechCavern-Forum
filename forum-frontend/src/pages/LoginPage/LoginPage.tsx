import React, { useRef, useState, useEffect, MutableRefObject } from "react";
import "./LoginPage.css";
import { FaUser, FaLock } from "react-icons/fa";
import { Link } from "react-router-dom";
import api from "../../utils/api";
import axios from "axios";
import { useAuth } from "../../contexts/AuthContext";

const LoginPage: React.FC = () => {
  const userRef = useRef() as MutableRefObject<HTMLInputElement>;

  const { setToken } = useAuth();
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
        localStorage.setItem("token", accessToken);

        setToken(accessToken);
      }

      setUsername("");
      setPassword("");
      setSuccess(true);
    } catch (err) {
      if (axios.isAxiosError(err)) {
        if (!err?.response) {
          setError("No Server Response");
        } else if (err.response?.status === 400) {
          setError(err.response?.data);
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
            {/*<div className="forgot-password">
              <Link to="/recovery">Forgot password?</Link>
            </div>*/}
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
