import { useEffect, useState } from "react";
import api from "../../utils/api";
import axios from "axios";
import { Link } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";
import decodeToken from "../../utils/tokenDecoder";

import { MdSubject } from "react-icons/md";
import { MdLocalPostOffice } from "react-icons/md";
import { MdMessage } from "react-icons/md";
import "./contactpage.css";

const ContactPage = () => {

  const { token } = useAuth();
  const [userId] = decodeToken(token!);
  const [subject, setSubject] = useState("");
  const [email, setEmail] = useState("");
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState<boolean>(false);

  useEffect(() => {
    setError("");
  }, [subject, email, message]);

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    try {
      await api.post("/utils/contact", {
        UserId: userId,
        Subject: subject,
        Email: email,
        Message: message,
      });

      setSuccess(true);
    }
    catch (err) {
      if (axios.isAxiosError(err)) {
        if (!err?.response) {
          setError("No Server Response");
        } else if (err.response?.status === 400) {
          setError(
            "Sending the message failed! The content in at least one of the input boxes is too long."
          );
        } else {
          setError("Sending the message failed!");
        }
        setSuccess(false);
      } else console.error(err);
    }
  };

  return (
    <div className="contact-wrapper">
      {success ? (
        <div className="wrapper-success">
          <h1>Success!</h1>
          <Link to="/" className="btn btn-primary">
            Go to home page
          </Link>
        </div>
      ) : (
        <div className="contact-container">
          <div className="title-div">
            <h1>Contact</h1>
          </div>
          {error && (
            <p id="errMsg" aria-live="assertive">
              {error}
            </p>
          )}
          <form onSubmit={handleSubmit}>
            <div className="input-box">
              <input
                type="text"
                id="subject"
                placeholder="Subject"
                autoComplete="off"
                value={subject}
                onChange={(e) => setSubject(e.target.value)}
                required
              />
              <MdSubject className="icon" />
            </div>
            <div className="input-box">
              <input
                type="email"
                id="email"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
              <MdLocalPostOffice className="icon" />
            </div>
            <div className="input-box">
              <input
                type="text"
                autoComplete="off"
                id="message"
                placeholder="Message"
                value={message}
                onChange={(e) => setMessage(e.target.value)}
                required
              />
              <MdMessage className="icon" />
            </div>
            <button type="submit">Send</button>
          </form>
        </div>)}
    </div>
  );
};

export default ContactPage;
