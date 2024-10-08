import {
  useParams,
  useNavigate,
  useSearchParams,
  Link,
} from "react-router-dom";
import { useState, useEffect } from "react";
import api from "../../utils/api";
import Pagination from "@mui/material/Pagination";

import "./postPage.css";
import timeAgo from "../../utils/timeAgo";
import { useAuth } from "../../contexts/AuthContext";
import decodeToken from "../../utils/tokenDecoder";

interface Post {
  id: number;
  title: string;
  content: string;
  userId: number;
  username: string;
  isPinned: boolean;
  isLocked: boolean;
  isEdited: boolean;
  isDeleted: boolean;
  isRemoved: boolean;
  lastEditedAt: string;
  createdAt: string;
  comments: Comment[];
  user: User;
}

interface Comment {
  id: number;
  content: string;
  userId: number;
  username: string;
  createdAt: string;
  isEdited: boolean;
  isDeleted: boolean;
  isRemoved: boolean;
  lastEditedAt: string;
  user: User;
}

interface User {
  selectedTitle: string;
  isActive: boolean;
  isBanned: boolean;
  createdAt: string;
  lastSeenOn: string;
  role: string;
}

const PostPage: React.FC = () => {
  const { token } = useAuth();
  const [userId, role] = decodeToken(token!);
  const { postId } = useParams<{ postId: string }>();
  const [post, setPost] = useState<Post | null>(null);
  const [loading, setLoading] = useState(true);
  const [isCooldown, setIsCooldown] = useState<boolean>(false);
  const [searchParams, setSearchParams] = useSearchParams();
  const [maxPagesCount, setMaxPagesCount] = useState<number>(1);
  const [page, setPage] = useState<number>(
    Number(searchParams.get("page")) || 1
  );
  const [content, setContent] = useState<string>("");
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const navigate = useNavigate();

  useEffect(() => {
    updateToValidPage();
    getPost();
    setSearchParams({ page: page.toString() }, { replace: true });
  }, [postId, page]);

  useEffect(() => {
    if (!loading && post === null) {
      navigate("/not-found", { replace: true });
    } else if (!loading && post) {
      updatePostViews();
    }
  }, [loading]);

  useEffect(() => {
    const pageFromQuery = Number(searchParams.get("page"));
    if (pageFromQuery && pageFromQuery !== page) {
      setPage(pageFromQuery);
    }
  }, [searchParams]);

  useEffect(() => {
    let timer: NodeJS.Timeout;
    if (isCooldown) {
      timer = setTimeout(() => {
        setIsCooldown(false);
      }, 10000);
    }
    return () => clearTimeout(timer);
  }, [isCooldown]);

  const updatePostViews = async () => {
    try {
      await api.patch(`/posts/${postId}/views`);
    } catch (err: any) {
      if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
        console.error("Error fetching page number for post:", err);
      }
    }
  };

  const updateToValidPage = async () => {
    try {
      const response = await api.get(`/posts/${postId}/max-pages`);
      if (response.data === 0) setMaxPagesCount(1);
      else setMaxPagesCount(response.data);
      if (page < 1 || page > maxPagesCount) {
        setPage(1);
        navigate(`/post/${postId}?page=${page}`, { replace: true });
      }
    } catch (err: any) {
      if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
        console.error("Error fetching page number for post:", err);
      }
    }
  };

  const getPost = async () => {
    try {
      const response = await api.get(`/posts/${postId}?page=${page}`);
      if (response.data) {
        setPost(response.data);
      } else {
        setPost(null);
      }
    } catch (err: any) {
      if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
        console.error("Error fetching post:", err);
      }
    } finally {
      setLoading(false);
    }
  };

  const handlePageChange = (
    _event: React.ChangeEvent<unknown>,
    value: number
  ) => {
    setPage(value);
  };

  const handleAddComment = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await api.post(`/comments`, { content, userId, postId });
      setContent("");
      updateToValidPage();
      setPage(maxPagesCount);
      setSearchParams({ page: maxPagesCount.toString() }, { replace: true });
      await getPost();
      setIsCooldown(true);
    } catch (err: any) {
      if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
        setErrorMessage("Error adding the comment. Please try again.");
        console.error("Error adding comment:", err);
      }
    }
  };

  return (
    <div className="post-wrapper">
      <div className="post-and-comments-container">
        {post ? (
          <>
            {page === 1 &&
              postContentContainer(
                post,
                Number.parseInt(userId),
                role,
                getPost
              )}
            <div className="all-post-comments-container">
              {post.comments.map((comment) =>
                commentContainer(
                  comment,
                  Number.parseInt(userId),
                  role,
                  getPost
                )
              )}
            </div>
            {!post.isLocked && (
              <div className="add-comment-wrapper">
                {errorMessage && <p className="errMsg">{errorMessage}</p>}
                <form onSubmit={handleAddComment}>
                  <div className="input-box">
                    <label htmlFor="content">ADD COMMENT</label>
                    <textarea
                      id="content"
                      value={content}
                      onChange={(e) => setContent(e.target.value)}
                      placeholder="Enter your comment"
                      required
                    ></textarea>
                  </div>
                  <div className="add-comment-button-container">
                    <button
                      type="submit"
                      className="add-comment-button"
                      disabled={isCooldown}
                    >
                      {isCooldown ? "Please wait..." : "ADD COMMENT"}
                    </button>
                  </div>
                </form>
              </div>
            )}
          </>
        ) : (
          <p className="loading-post">Fetching post...</p>
        )}
        <div className="pagination-bar-div">
          <Pagination
            count={maxPagesCount}
            page={page}
            onChange={handlePageChange}
            showFirstButton
            showLastButton
            color="primary"
            className="pagination-bar"
          />
        </div>
      </div>
    </div>
  );
};

const postContentContainer = (
  post: Post,
  userId: number,
  role: string,
  getPost: Function
) => {
  const deletePost = async () => {
    try {
      await api.delete(`/posts/${post.id}`);
      await getPost();
    } catch (err: any) {
      if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
        console.error("Error deleting comment:", err);
      }
    }
  };

  const removePost = async () => {
    try {
      await api.delete(`/posts/remove/${post.id}`);
      await getPost();
    } catch (err: any) {
      if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
        console.error("Error deleting comment:", err);
      }
    }
  };

  const updatePostPinStatus = async () => {
    try {
      await api.patch(`/posts/update-pin/${post.id}`);
      await getPost();
    } catch (err: any) {
      if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
        console.error("Error deleting comment:", err);
      }
    }
  };

  const updatePostLockStatus = async () => {
    try {
      await api.patch(`/posts/update-lock/${post.id}`);
      await getPost();
    } catch (err: any) {
      if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
        console.error("Error deleting comment:", err);
      }
    }
  };

  return (
    <>
      <div className="post-title-and-buttons-container">
        <div className="post-title-div">
          <p className="post-title">{post.title}</p>
        </div>
        {role === "Admin" &&
          post.isDeleted === false &&
          post.isRemoved === false && (
            <div className="post-admin-buttons">
              <button
                onClick={updatePostPinStatus}
                className={
                  post.isPinned === true
                    ? "admin-unpin-button"
                    : "admin-pin-button"
                }
              >
                {post.isPinned === true && "UN"}PIN POST
              </button>
              <button
                onClick={updatePostLockStatus}
                className={
                  post.isLocked ? "admin-unlock-button" : " admin-lock-button"
                }
              >
                {post.isLocked && "UN"}LOCK POST
              </button>
            </div>
          )}
      </div>
      <div className="post-page-content-container">
        <div className="post-left-container">
          <div className="post-author">
            <p className="post-author-username">
              <Link to={"/user/profile/" + post.userId}>{post.username}</Link>
              <span
                className={`status-indicator ${
                  post.user.isActive ? "active" : "inactive"
                }`}
              ></span>
            </p>
            <div className="user-extra-info">
              <p>Title: {post.user.selectedTitle}</p>
              <p>Role: {post.user.role}</p>
              {!post.user.isActive && (
                <p>Last seen {timeAgo(post.user.lastSeenOn)}</p>
              )}
              {post.user.isBanned && <p>Banned</p>}
            </div>
          </div>
        </div>
        <div className="post-right-container">
          <div className="post-time-and-buttons">
            <div className="post-time-container">
              <p>{"Posted " + timeAgo(post.createdAt)}</p>
              {post.isEdited && (
                <p>{"Last edited  " + timeAgo(post.lastEditedAt)}</p>
              )}
            </div>
            {post.userId === userId &&
            post.isDeleted === false &&
            post.isRemoved === false ? (
              <div className="post-buttons">
                {" "}
                <Link to={`/edit-post/${post.id}`} className="edit-post-button">
                  EDIT POST
                </Link>
                <button onClick={deletePost} className="delete-post-button">
                  DELETE POST
                </button>
              </div>
            ) : (
              role === "Admin" &&
              post.isDeleted === false &&
              post.isRemoved === false && (
                <div className="post-buttons">
                  {" "}
                  <Link
                    to={`/edit-post/${post.id}`}
                    className="edit-post-button"
                  >
                    EDIT POST
                  </Link>
                  <button onClick={removePost} className="delete-post-button">
                    REMOVE POST
                  </button>
                </div>
              )
            )}
          </div>
          <div className="post-page-separator-container">
            <div className="post-page-separator"></div>
          </div>
          <div className="post-content">
            <p>{post.content}</p>
          </div>
        </div>
      </div>
    </>
  );
};

const commentContainer = (
  comment: Comment,
  userId: number,
  role: string,
  getPost: Function
) => {
  const deleteComment = async () => {
    try {
      await api.delete(`/comments/${comment.id}`);
      await getPost();
    } catch (err: any) {
      if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
        console.error("Error deleting comment:", err);
      }
    }
  };

  const removeComment = async () => {
    try {
      await api.delete(`/comments/remove/${comment.id}`);
      await getPost();
    } catch (err: any) {
      if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
        console.error("Error deleting comment:", err);
      }
    }
  };

  return (
    <div
      className="post-comment-content-container"
      key={`comment-${comment.id}`}
    >
      <div className="post-comment-left-container">
        <div className="post-comment-author">
          <p className="post-comment-author-username">
            <Link to={"/user/profile/" + comment.userId}>
              {comment.username}
            </Link>
            <span
              className={`status-indicator ${
                comment.user.isActive ? "active" : "inactive"
              }`}
            ></span>
          </p>
          <div className="user-extra-info">
            <p>Title: {comment.user.selectedTitle}</p>
            <p>Role: {comment.user.role}</p>
            {!comment.user.isActive && (
              <p>Last seen {timeAgo(comment.user.lastSeenOn)}</p>
            )}
            {comment.user.isBanned && <p>Banned</p>}
          </div>
        </div>
      </div>
      <div className="post-comment-right-container">
        <div className="post-comment-time-and-buttons">
          <div className="post-comment-time-container">
            <p>{"Posted " + timeAgo(comment.createdAt)}</p>
            {comment.isEdited && (
              <p>{"Last edited  " + timeAgo(comment.lastEditedAt)}</p>
            )}
          </div>
          {comment.userId === userId &&
          comment.isDeleted === false &&
          comment.isRemoved === false ? (
            <div className="post-comment-buttons">
              {" "}
              <Link
                to={`/edit-comment/${comment.id}`}
                className="edit-comment-button"
              >
                EDIT COMMENT
              </Link>
              <button onClick={deleteComment} className="delete-comment-button">
                DELETE COMMENT
              </button>
            </div>
          ) : (
            role === "Admin" &&
            comment.isDeleted === false &&
            comment.isRemoved === false && (
              <div className="post-comment-buttons">
                {" "}
                <Link
                  to={`/edit-comment/${comment.id}`}
                  className="edit-comment-button"
                >
                  EDIT COMMENT
                </Link>
                <button
                  onClick={removeComment}
                  className="delete-comment-button"
                >
                  REMOVE COMMENT
                </button>
              </div>
            )
          )}
        </div>
        <div className="post-comment-separator-container">
          <div className="post-comment-separator"></div>
        </div>
        <div className="post-comment-content">
          <p>{comment.content}</p>
        </div>
      </div>
    </div>
  );
};

export default PostPage;
