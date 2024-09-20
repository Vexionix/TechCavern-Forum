import Message from "../../Message.tsx";
import "../../App.css";
import { Link } from "react-router-dom";

function HomePage() {
  return (
    <div>
      <Message message="hello there on the main page" />
      <Link to="/login">login page</Link>
    </div>
  );
}

export default HomePage;
