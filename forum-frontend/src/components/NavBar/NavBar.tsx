import { Link } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";

import "./navbar.css";
import { FaHome } from "react-icons/fa";
import { FaPerson } from "react-icons/fa6";
import { FaQuestion } from "react-icons/fa";
import { FaBook } from "react-icons/fa";
import { FaCrown } from "react-icons/fa";

const NavBar = () => {
  const { role } = useAuth();
  const currentUrl: string = window.location.href.slice(22);

  return (
    <ul className="navbar-ul">
      <li className={currentUrl === "/" ? "navbar-li selected" : "navbar-li"}>
        <Link to="/">
          <FaHome /> Home
        </Link>
      </li>
      <li
        className={currentUrl === "/rules" ? "navbar-li selected" : "navbar-li"}
      >
        <Link to="/rules">
          <FaBook /> Rules
        </Link>
      </li>
      <li
        className={currentUrl === "/faq" ? "navbar-li selected" : "navbar-li"}
      >
        <Link to="/faq">
          <FaQuestion /> FAQ
        </Link>
      </li>
      <li
        className={currentUrl === "/staff" ? "navbar-li selected" : "navbar-li"}
      >
        <Link to="/staff">
          <FaCrown /> Staff
        </Link>
      </li>
      {role === "Admin" && (
        <li
          className={
            currentUrl === "/users" ? "navbar-li selected" : "navbar-li"
          }
        >
          <Link to="/users">
            <FaPerson /> Users
          </Link>
        </li>
      )}
    </ul>
  );
};

export default NavBar;
