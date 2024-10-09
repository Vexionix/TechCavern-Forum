import { useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import api from "../../utils/api";

import "./editProfilePage.css";
import { useAuth } from "../../contexts/AuthContext";
import decodeToken from "../../utils/tokenDecoder";

const EditProfilePage: React.FC = () => {
  const { token } = useAuth();
  const [currentUserId, role] = decodeToken(token!);
  const navigate = useNavigate();
  const { userId } = useParams<{ userId: string }>();
  const [title, setTitle] = useState<string>("");
  const [bio, setBio] = useState<string>("");
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [unlockedTitles, setUnlockedTitles] = useState<string[]>([]);

  useEffect(() => {
    getUserData();
    getUnlockedTitles();
  }, []);

  useEffect(() => {
    setErrorMessage("");
  }, [bio]);

  const isValidBio = () => {
    return bio.length <= 250;
  };

  const getUserData = async () => {
    try {
      const response = await api.get(`/users/${userId}`);
      if (userId !== currentUserId && role !== "Admin") {
        navigate("/forbidden", { replace: true });
      }
      setBio(response.data.bio);
      setTitle(response.data.selectedTitle);
    } catch (err: any) {
      if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
        console.error("Error deleting comment:", err);
      }
    }
  };

  const getUnlockedTitles = async () => {
    try {
      const response = await api.get(`/users/titles/${userId}`);
      setUnlockedTitles(response.data);
    } catch (err: any) {
      if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
        console.error("Error getting unlocked titles:", err);
      }
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!isValidBio()) {
      setErrorMessage("Bio must be up to 250 characters.");
      return;
    }
    try {
      const response = await api.put(`/users/${userId}/update-profile`, {
        selectedTitle: title,
        bio: bio,
      });

      if (response.status === 200) {
        navigate(-1);
      }
    } catch (error) {
      setErrorMessage("Error editing the profile. Please try again.");
    }
  };

  return (
    <div className="edit-profile-body">
      <div className="edit-profile-wrapper">
        <h2>Edit Profile</h2>
        {errorMessage && <p className="errMsg">{errorMessage}</p>}
        <form onSubmit={handleSubmit}>
          <div className="input-box">
            <label htmlFor="title">Title</label>
            <select
              id="title"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              required
            >
              <option value="" disabled>
                Select your title
              </option>
              {unlockedTitles.map((unlockedTitle, index) => (
                <option key={"user-title" + index} value={unlockedTitle}>
                  {unlockedTitle}
                </option>
              ))}
            </select>
          </div>
          <div className="input-box">
            <label htmlFor="bio">Bio</label>
            <textarea
              id="bio"
              value={bio}
              onChange={(e) => setBio(e.target.value)}
              placeholder="Enter your new bio"
              required
            ></textarea>
          </div>
          <button type="submit" className="btn btn-primary">
            EDIT PROFILE
          </button>
        </form>
      </div>
    </div>
  );
};

export default EditProfilePage;
