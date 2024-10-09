import { useEffect, useState } from "react";
import "./staffPage.css";
import api from "../../utils/api";
import timeAgo from "../../utils/timeAgo";
import { Link } from "react-router-dom";

interface User {
  id: number;
  username: string;
  selectedTitle: string;
  isActive: boolean;
  createdAt: string;
  lastSeenOn: string;
  role: string;
  bio: string;
}

const StaffPage: React.FC = () => {
  const [users, setUsers] = useState<User[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    getUsers();
  }, []);

  const getUsers = async () => {
    try {
      const response = await api.get(`/users/staff`);
      setUsers(response.data);
      setIsLoading(false);
    } catch (err: any) {
      if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
        console.error("Error fetching staff:", err);
      }
    }
  };

  return (
    <div className="staff-wrapper">
      {!isLoading ? (
        users!.map((user) => {
          return (
            <div className="staff-container" key={`staff-${user.id}`}>
              <div className="staff-member">
                <p className="staff-member-username">
                  <Link to={"/user/profile/" + user.id}>{user.username}</Link>
                  <span
                    className={`status-indicator ${
                      user.isActive ? "active" : "inactive"
                    }`}
                  ></span>
                </p>
                <div className="user-extra-info">
                  <p>Title: {user.selectedTitle}</p>
                  <p>Role: {user.role}</p>
                  {!user.isActive && (
                    <p>Last seen {timeAgo(user.lastSeenOn)}</p>
                  )}
                </div>
                <div className="user-extra-info">
                  <p>About them:</p>
                  <p>{user.bio}</p>
                </div>
              </div>
            </div>
          );
        })
      ) : (
        <p>Loading...</p>
      )}
    </div>
  );
};

export default StaffPage;
