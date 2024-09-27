import { useState, useEffect } from "react";
import api from "../../utils/api";

import "./Users.css";

interface UserModel {
  username: string;
  role: number;
}

const Users = () => {
  const [users, setUsers] = useState<UserModel[]>();

  useEffect(() => {
    let isMounted = true;
    const controller = new AbortController();

    const getUsers = async () => {
      try {
        const response = await api.get("/users", {
          signal: controller.signal,
        });
        isMounted && setUsers(response.data);
      } catch (err) {
        console.error(err);
      }
    };

    getUsers();

    return () => {
      isMounted = false;
      controller.abort;
    };
  }, []);

  return (
    <div className="users-body">
      <article>
        <h2>Users List</h2>
        {users?.length ? (
          <ul>
            {" "}
            {users.map((user, i) => (
              <li key={i}>
                {user?.username} - {user?.role === 0 ? "Member" : "Admin"}
              </li>
            ))}
          </ul>
        ) : (
          <p>No users to display</p>
        )}
      </article>
    </div>
  );
};

export default Users;
