import { useAuth } from "../../contexts/AuthContext.tsx";
import Message from "../../Message.tsx";
import "./HomePage.css";
import { Link } from "react-router-dom";

function HomePage() {
  const { logout } = useAuth();

  return (
    <div className="home-body">
      <div className="home-wrapper">
        <div className="top-container">
          <div className="top-left-element">
            <p>a</p>
          </div>
          <div className="top-right-element">
            <p>b</p>
          </div>
        </div>
        <div className="bottom-container">
          <div className="bottom-left-element">
            <p>c</p>
          </div>
          <div className="bottom-right-container">
            <div className="bottom-right-first-element">
              <p>d</p>
            </div>
            <div className="bottom-right-second-element">
              <p>e</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default HomePage;
