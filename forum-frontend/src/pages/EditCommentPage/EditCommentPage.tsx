import { useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import api from "../../utils/api";

import "./editCommentPage.css";
import { useAuth } from "../../contexts/AuthContext";
import decodeToken from "../../utils/tokenDecoder";

const EditCommentPage: React.FC = () => {
  const { token } = useAuth();
  const [userId, role] = decodeToken(token!);
  const navigate = useNavigate();
  const { commentId } = useParams<{ commentId: string }>();
  const [content, setContent] = useState<string>("");
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  useEffect(() => {
    api.get(`/comments/${commentId}`).then((response) => {
      if (response.status === 200) {
        if (response.data.userId.toString() !== userId && role !== "Admin") {
          navigate("/forbidden", { replace: true });
        }
        setContent(response.data.content);
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

    if (!isValidContent(content)) {
      setErrorMessage(
        "Content must be up to 500 characters and can only include letters, digits, and punctuation."
      );
      return;
    }

    try {
      const response = await api.put(`/comments/edit/${commentId}`, {
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
    <div className="edit-comment-body">
      <div className="edit-comment-wrapper">
        <h2>Edit Comment</h2>
        {errorMessage && <p className="errMsg">{errorMessage}</p>}
        <form onSubmit={handleSubmit}>
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
            EDIT COMMENT
          </button>
        </form>
      </div>
    </div>
  );
};

export default EditCommentPage;
