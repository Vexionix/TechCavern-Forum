import { Link } from "react-router-dom";

import "./footer.css";
import { FaLinkedin } from "react-icons/fa";
import { FaGithub } from "react-icons/fa";
import { FaCopyright } from "react-icons/fa";

const Footer = () => {
  return (
    <div className="footer-body">
      <div className="footer-row-upper">
        <div className="footer-col">
          <h4>About</h4>
          <p className="about-purpose">
            This forum was created in order to provide an easy way for users to
            discuss tech related subjects.
          </p>
          <p className="about-technologies">
            Technologies used: DOTNET for Backend, React for Frontend
          </p>
        </div>
        <div className="footer-col">
          <h4>Links</h4>
          <ul>
            <li>
              <Link to="/rules">Rules</Link>
            </li>
            <li>
              <Link to="/faq">FAQ</Link>
            </li>
            <li>
              <Link to="/contact">Contact</Link>
            </li>
          </ul>
        </div>
        <div className="footer-col">
          <div className="social-links">
            <h4>Founder's Social Media</h4>
            <Link to="https://www.linkedin.com/in/perju-mircea-stefan/">
              <FaLinkedin />
            </Link>
            <Link to="https:///www.github.com/Vexionix">
              <FaGithub />
            </Link>
          </div>
        </div>
      </div>
      <p className="footer-separator"></p>
      <div className="footer-row-lower">
        <div className="copyright-notice">
          <p className="year-text">
            <FaCopyright /> {new Date().getFullYear()}
          </p>
          <p className="name-text">Perju Mircea-Stefan</p>
        </div>
      </div>
    </div>
  );
};

export default Footer;
