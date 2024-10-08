import NavBar from "../NavBar/NavBar";
import { Link } from "react-router-dom";
import { useState, useEffect } from "react";
import api from "../../utils/api";

import "./header.css";
import Tooltip from "@mui/material/Tooltip";
import { FaComputer } from "react-icons/fa6";
import { HiIdentification } from "react-icons/hi";
import { HiLogout } from "react-icons/hi";
import { IoMdSettings } from "react-icons/io";
import decodeToken from "../../utils/tokenDecoder";
import { useAuth } from "../../contexts/AuthContext";

const Header = () => {
  const { token, logout } = useAuth();
  const [userId] = decodeToken(token!);

  const [activeUsers, setActiveUsers] = useState(-1);
  const [postsToday, setPostsToday] = useState(-1);

  useEffect(() => {
    const controller = new AbortController();

    const getPostsAddedToday = async () => {
      try {
        const response = await api.get("/posts/added-today", {
          signal: controller.signal,
        });
        setPostsToday(response.data);
      } catch (err) {
        console.error(err);
      }
    };

    const getActiveUsers = async () => {
      try {
        const response = await api.get("/users/active", {
          signal: controller.signal,
        });
        setActiveUsers(response.data);
      } catch (err) {
        console.error(err);
      }
    };

    getPostsAddedToday();
    getActiveUsers();

    return () => {
      controller.abort;
    };
  }, []);

  return (
    <div className="header-body">
      <NavBar />
      <div className="center-container">
        <div className="header-active-users">
          <p>Active users:</p>
          <p className="active-users-value">
            {activeUsers >= 0 ? activeUsers : "-"}
          </p>
        </div>
        <div className="header-logo">
          <p>TECHCAVERN</p>
          <FaComputer />
        </div>
        <div className="header-posts-today">
          <p>Posts added today:</p>
          <p className="posts-today-value">
            {postsToday >= 0 ? postsToday : "-"}
          </p>
        </div>
      </div>
      <div className="header-shortcuts">
        <Tooltip title="Profile">
          <Link to={"/user/profile/" + userId}>
            <HiIdentification />
          </Link>
        </Tooltip>
        <Tooltip title="Logout">
          <span>
            <HiLogout className="logout-icon" onClick={() => logout()} />
          </span>
        </Tooltip>
      </div>
      <p className="header-separator"></p>
    </div>
  );
};

export default Header;
