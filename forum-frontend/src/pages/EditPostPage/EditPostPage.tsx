import { useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import api from "../../utils/api";

import "./editPostPage.css";
import { useAuth } from "../../contexts/AuthContext";
import decodeToken from "../../utils/tokenDecoder";

const EditPostPage: React.FC = () => {
  const { token } = useAuth();
  const [userId, role] = decodeToken(token!);
  const navigate = useNavigate();
  const { postId } = useParams<{ postId: string }>();
  const [title, setTitle] = useState<string>("");
  const [content, setContent] = useState<string>("");
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  useEffect(() => {
    api.get(`/posts/${postId}`).then((response) => {
      if (response.status === 200) {
        if (response.data.userId.toString() !== userId && role !== "Admin") {
          navigate("/forbidden", { replace: true });
        }
        setContent(response.data.content);
        setTitle(response.data.title);
      }
    });
  }, []);

  useEffect(() => {
    setErrorMessage("");
  }, [content]);

  const isValidContent = (content: string) => {
    //const contentPattern = /^[A-Za-z0-9\s.,?!;]*$/;
    return content.length <= 500; // && contentPattern.test(content);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (title.length > 50) {
      setErrorMessage("Title must be up to 50 characters.");
      return;
    }

    if (!isValidContent(content)) {
      setErrorMessage("Content must be up to 500 characters.");
      return;
    }

    try {
      const response = await api.put(`/posts/edit/${postId}`, {
        title: title,
        content: content,
      });

      if (response.status === 200) {
        navigate(-1);
      }
    } catch (error) {
      setErrorMessage("Error editing the comment. Please try again.");
    }
  };

  return (
    <div className="edit-post-body">
      <div className="edit-post-wrapper">
        <h2>Edit Post</h2>
        {errorMessage && <p className="errMsg">{errorMessage}</p>}
        <form onSubmit={handleSubmit}>
          <div className="input-box">
            <label htmlFor="title">Title</label>
            <input
              id="title"
              type="text"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              placeholder="Enter your post title"
              required
            />
          </div>
          <div className="input-box">
            <label htmlFor="content">Content</label>
            <textarea
              id="content"
              value={content}
              onChange={(e) => setContent(e.target.value)}
              placeholder="Enter your new comment"
              required
            ></textarea>
          </div>
          <button type="submit" className="btn btn-primary">
            EDIT POST
          </button>
        </form>
      </div>
    </div>
  );
};

export default EditPostPage;
