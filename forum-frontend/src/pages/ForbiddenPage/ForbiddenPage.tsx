import "./ForbiddenPage.css";
import { Link } from "react-router-dom";

const ForbiddenPage = () => {
  return (
    <div className="forbidden-body">
      <div className="forbidden-wrapper">
        <p className="error-code">403</p>
        <p className="forbidden-text">Forbidden</p>
        <p className="error-info">
          Access to this resource on the server is denied!{" "}
        </p>
        <Link to="/">Click here to go to the main page.</Link>
      </div>
    </div>
  );
};

export default ForbiddenPage;
