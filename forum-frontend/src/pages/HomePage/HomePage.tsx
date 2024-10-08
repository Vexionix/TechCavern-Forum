import { useEffect, useState } from "react";
import "./HomePage.css";
import api from "../../utils/api";
import { useAuth } from "../../contexts/AuthContext";
import decodeToken from "../../utils/tokenDecoder";
import Categories from "../../components/Categories/Categories";
import Statistics from "../../components/Statistics/Statistics";
import LatestPosts from "../../components/LatestPosts/LatestPosts";

function HomePage() {
  const { token } = useAuth();
  const [userId] = decodeToken(token!);

  const [username, setUsername] = useState("");

  useEffect(() => {
    const controller = new AbortController();

    const getUsername = async () => {
      try {
        const response = await api.get("/users/" + userId + "/username", {
          signal: controller.signal,
        });
        setUsername(response.data);
      } catch (err: any) {
        if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
          console.error("Error fetching username:", err);
        }
      }
    };

    getUsername();

    return () => {
      controller.abort();
    };
  }, []);

  return (
    <div className="home-body">
      <div className="home-wrapper">
        <div className="top-container">
          <div className="top-element">
            <p>
              Hello, <span className="greeting-username">{username}</span>!
            </p>
          </div>
        </div>
        <div className="bottom-container">
          <div className="bottom-left-element">
            <Categories />
          </div>
          <div className="bottom-right-container">
            <div className="bottom-right-first-element">
              <Statistics />
            </div>
            <div className="bottom-right-second-element">
              <LatestPosts />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default HomePage;
