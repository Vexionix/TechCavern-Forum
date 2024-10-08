import { useParams, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import api from "../../utils/api";

import "./AddPostPage.css";
import { useAuth } from "../../contexts/AuthContext";
import decodeToken from "../../utils/tokenDecoder";

const AddPostPage: React.FC = () => {
  const { subcategoryId } = useParams<{ subcategoryId: string }>();
  const { token } = useAuth();
  const [userId] = decodeToken(token!);
  const navigate = useNavigate();
  const [title, setTitle] = useState<string>("");
  const [content, setContent] = useState<string>("");
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  useEffect(() => {
    setErrorMessage("");
  }, [title, content]);

  /*const isValidTitle = (title: string) => {
    const titlePattern = /^[A-Za-z0-9\s]{1,50}$/;
    return titlePattern.test(title);
  };

  const isValidContent = (content: string) => {
    const contentPattern = /^[A-Za-z0-9\s.,?!;]*$/;
    return contentPattern.test(content) && content.length <= 500;
  };*/

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    /*if (!isValidTitle(title)) {
      setErrorMessage(
        "Title must be up to 50 characters and contain only letters and digits."
      );
      return;
    }

    if (!isValidContent(content)) {
      setErrorMessage(
        "Content must be up to 500 characters and can only include letters, digits, and punctuation."
      );
      return;
    }*/

    if (title.length > 50) {
      setErrorMessage("Title must be up to 50 characters.");
      return;
    }

    if (content.length > 500) {
      setErrorMessage("Content must be up to 500 characters.");
      return;
    }

    try {
      const response = await api.post("/posts", {
        title,
        content,
        userId,
        subcategoryId,
      });

      if (response.status === 201) {
        navigate(`/subcategory/${subcategoryId}`);
      }
    } catch (error) {
      setErrorMessage("Error adding the post. Please try again.");
    }
  };

  return (
    <div className="add-post-body">
      <div className="add-post-wrapper">
        <h2>Add a New Post</h2>
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
              placeholder="Enter your post content"
              required
            ></textarea>
          </div>
          <button type="submit" className="btn btn-primary">
            ADD POST
          </button>
        </form>
      </div>
    </div>
  );
};

export default AddPostPage;
