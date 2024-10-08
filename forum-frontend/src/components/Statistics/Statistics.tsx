import { useEffect, useState } from "react";
import api from "../../utils/api";

import "./statistics.css";
import { Link } from "react-router-dom";

interface Statistics {
  totalPosts: number;
  totalComments: number;
  totalUsers: number;
  latestUserId: number;
  latestUsername: string;
}

const Statistics = () => {
  const [statistics, setStatistics] = useState<Statistics>({
    totalPosts: 0,
    totalComments: 0,
    totalUsers: 0,
    latestUserId: 0,
    latestUsername: "",
  });

  useEffect(() => {
    const controller = new AbortController();

    const getStatistics = async () => {
      try {
        const response = await api.get("/utils/statistics", {
          signal: controller.signal,
        });
        setStatistics(response.data);
      } catch (err: any) {
        if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
          console.error("Error fetching categories:", err);
        }
      }
    };

    getStatistics();

    return () => {
      controller.abort();
    };
  }, []);

  return (
    <div className="statistics-wrapper">
      <div className="statistics-title">
        <p>Statistics</p>
      </div>
      <div className="statistics-content">
        <div className="statistics-element">
          <p className="statistics-description">Total Posts:</p>
          <p className="statistics-value">{statistics.totalPosts}</p>
        </div>
        <div className="statistics-element">
          <p className="statistics-description">Total Comments:</p>
          <p className="statistics-value">{statistics.totalComments}</p>
        </div>
        <div className="statistics-element">
          <p className="statistics-description">Total Users:</p>
          <p className="statistics-value">{statistics.totalUsers}</p>
        </div>
        {statistics.latestUserId !== 0 && (
          <>
            <div className="statistics-separator"></div>{" "}
            <div className="statistics-latest-element">
              <p className="statistics-description">Latest User: </p>
              <Link
                to={`/user/profile/${statistics.latestUserId}`}
                className="statistics-latest-user"
              >
                {statistics.latestUsername}
              </Link>
            </div>
          </>
        )}
      </div>
    </div>
  );
};

export default Statistics;
