import { useAuth } from "../../contexts/AuthContext.tsx";
import Message from "../../Message.tsx";
import "./HomePage.css";
import { Link } from "react-router-dom";

function HomePage() {
  const { logout } = useAuth();

  return (
    <div className="home-body">
      <div className="home-wrapper">
        <Message message="hello there on the main page" />
        <Link to="/login">login page</Link>
        <button className="btn btn-primary" onClick={logout}>
          Logout
        </button>
      </div>
    </div>
  );
}

export default HomePage;
