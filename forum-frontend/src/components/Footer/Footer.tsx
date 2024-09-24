import { Link } from "react-router-dom";

import "./footer.css";
import { FaLinkedin } from "react-icons/fa";
import { FaGithub } from "react-icons/fa";

const Footer = () => {
  return (
    <div className="footer-body">
      <div className="footer-row">
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
              <a href="#">Rules</a>
            </li>
            <li>
              <a href="#">FAQ</a>
            </li>
            <li>
              <a href="#">Contact</a>
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
    </div>
  );
};

export default Footer;
