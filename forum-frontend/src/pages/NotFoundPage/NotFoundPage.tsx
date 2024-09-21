import "./NotFoundPage.css";
import { Link } from "react-router-dom";

const NotFoundPage = () => {
  return (
    <div className="not-found-body">
      <div className="not-found-wrapper">
        <p className="error-emoji">ʘ︵ʘ</p>
        <p className="error-code">404</p>
        <p className="not-found-text">Page not found</p>
        <p className="error-info">
          The page you are looking for doesn't exist or an other error occured.{" "}
          <Link to="/">Click here to go to the main page.</Link>
        </p>
      </div>
    </div>
  );
};

export default NotFoundPage;
