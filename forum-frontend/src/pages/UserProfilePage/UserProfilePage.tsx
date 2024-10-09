import { useEffect, useState } from "react";
import "./userProfilePage.css";
import timeAgo from "../../utils/timeAgo";
import { Link, useNavigate, useParams } from "react-router-dom";
import api from "../../utils/api";
import formatDateString from "../../utils/dateFormatter";
import { useAuth } from "../../contexts/AuthContext";
import decodeToken from "../../utils/tokenDecoder";

interface User {
  id: number;
  username: string;
  selectedTitle: string;
  isActive: boolean;
  isBanned: boolean;
  createdAt: string;
  lastSeenOn: string;
  totalPosts: number;
  totalComments: number;
  role: string;
  bio: string;
}

interface Post {
  id: number;
  title: string;
  createdAt: string;
}

interface Comment {
  id: number;
  content: string;
  createdAt: string;
  postId: number;
  title: string;
}

const UserProfilePage: React.FC = () => {
  const { token } = useAuth();
  const [currentUserId, role] = decodeToken(token!);
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [posts, setPosts] = useState<Post[]>([]);
  const [comments, setComments] = useState<Comment[]>([]);
  const userId = useParams<{ userId: string }>().userId;
  const navigate = useNavigate();

  useEffect(() => {
    getUser();
    getPosts();
    getComments();
  }, []);

  useEffect(() => {
    getUser();
    getPosts();
    getComments();
  }, [userId]);

  const getUser = async () => {
    try {
      const response = await api.get(`/users/${userId}/profile-data`);
      if (response.data) {
        setUser(response.data);
      } else {
        setUser(null);
      }
    } catch (err: any) {
      if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
        console.error("Error fetching user:", err);
      }
    } finally {
      setIsLoading(false);
    }
  };

  const getPosts = async () => {
    try {
      const response = await api.get(`/posts/latest/${userId}`);
      if (response.data) {
        setPosts(response.data);
      }
    } catch (err: any) {
      if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
        console.error("Error fetching posts:", err);
      }
    }
  };

  const getComments = async () => {
    try {
      const response = await api.get(`/comments/latest/${userId}`);
      if (response.data) {
        setComments(response.data);
      }
    } catch (err: any) {
      if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
        console.error("Error fetching comments:", err);
      }
    }
  };

  const updateBanStatus = async () => {
    try {
      await api.patch(`/users/${userId}/${user?.isBanned ? "unban" : "ban"}`);
      getUser();
      getPosts();
      getComments();
    } catch (err: any) {
      if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
        console.error("Error updating ban status:", err);
      }
    }
  };

  return (
    <div className="user-profile-wrapper">
      <div className="user-profile-data">
        {!isLoading && user != null ? (
          <>
            <div className="user-profile-header">
              <div className="user-info">
                <p className="username-p">
                  {user.username}
                  <span
                    className={`status-indicator ${
                      user.isActive ? "active" : "inactive"
                    }`}
                  ></span>
                </p>
                {user?.isBanned && <p className="banned-notice">Banned</p>}
                <div className="user-profile-extra-info">
                  <p className="user-title">Title: {user?.selectedTitle}</p>
                  <p className="user-role">Role: {user?.role}</p>
                  <p className="user-total-activity">
                    Total posts: {user?.totalPosts}
                  </p>
                  <p className="user-total-activity">
                    Total comments: {user?.totalComments}
                  </p>
                  <div className="time-info">
                    {!user?.isActive && (
                      <p className="last-seen">
                        Last seen {timeAgo(user.lastSeenOn)}
                      </p>
                    )}
                    <p className="joined-on">
                      Joined on {formatDateString(user.createdAt)}
                    </p>
                  </div>
                </div>
              </div>
              <div className="user-bio-container">
                <p className="user-bio">{user.bio}</p>
              </div>
            </div>
            <div className="user-profile-buttons">
              {(parseInt(currentUserId) === user.id || role === "Admin") && (
                <button
                  onClick={() => navigate("/edit-profile/" + user.id)}
                  className="edit-profile-button"
                >
                  EDIT PROFILE
                </button>
              )}
              {role === "Admin" && parseInt(currentUserId) !== user.id && (
                <button
                  onClick={updateBanStatus}
                  className={
                    user.isBanned === true
                      ? "admin-unban-button"
                      : "admin-ban-button"
                  }
                >
                  {user.isBanned === true ? "UNBAN" : "BAN"}
                </button>
              )}
            </div>
          </>
        ) : (
          <div className="user-profile-header">
            <p>Loading...</p>
          </div>
        )}

        <div className="user-profile-latest-activity">
          <div className="user-profile-latest-posts">
            <h3>Latest Posts</h3>
            <div className="profile-latest-posts-content"></div>
            {posts.length > 0 ? (
              posts.map((post) => {
                return (
                  <div
                    key={"post-" + post.id}
                    className="profile-latest-posts-element"
                  >
                    <div className="profile-latest-posts">
                      <Link to={"/post/" + post.id} className="post-link">
                        {post.title}
                      </Link>
                      <div className="post-details">
                        <p>
                          <span className="post-time">
                            {timeAgo(post.createdAt)}{" "}
                          </span>
                        </p>
                      </div>
                    </div>
                  </div>
                );
              })
            ) : (
              <p>No posts found</p>
            )}
          </div>
          <div className="user-profile-latest-comments">
            <h3>Latest Comments</h3>
            {comments.length > 0 ? (
              comments.map((comment) => {
                return (
                  <div
                    key={"user-latest-comment-" + comment.id}
                    className="latest-comments-element"
                  >
                    <p>{comment.content}</p>
                    <div className="latest-comments">
                      <p>
                        <Link
                          to={"/post/" + comment.postId}
                          className="comment-link"
                        >
                          {comment.title}
                        </Link>
                        <span className="comment-time">
                          {" "}
                          ● {timeAgo(comment.createdAt)}{" "}
                        </span>
                      </p>
                    </div>
                  </div>
                );
              })
            ) : (
              <p>No comments found</p>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default UserProfilePage;
