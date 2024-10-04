import "./RegisterPage.css";
import { FaUser, FaLock, FaEnvelope } from "react-icons/fa";
import {
  useRef,
  useState,
  useEffect,
  SyntheticEvent,
  MutableRefObject,
} from "react";
import { Link } from "react-router-dom";
import {
  faCheck,
  faTimes,
  faInfoCircle,
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import api from "../../utils/api";
import axios from "axios";

const USER_REGEX = /^[a-zA-Z][a-zA-Z0-9_]{3,23}$/;
const PWD_REGEX = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%])[\S]{8,24}$/;
const EMAIL_REGEX =
  /^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$/;

const RegisterPage = () => {
  const userRef = useRef() as MutableRefObject<HTMLInputElement>;

  const [user, setUser] = useState("");
  const [validName, setValidName] = useState(false);
  const [userFocus, setUserFocus] = useState(false);

  const [email, setEmail] = useState("");
  const [validEmail, setValidEmail] = useState(false);
  const [emailFocus, setEmailFocus] = useState(false);

  const [pwd, setPwd] = useState("");
  const [validPwd, setValidPwd] = useState(false);
  const [pwdFocus, setPwdFocus] = useState(false);

  const [matchPwd, setMatchPwd] = useState("");
  const [validMatch, setValidMatch] = useState(false);
  const [matchFocus, setMatchFocus] = useState(false);

  const [errMsg, setErrMsg] = useState("");
  const [success, setSuccess] = useState(false);

  useEffect(() => {
    userRef.current?.focus();
  }, []);

  useEffect(() => {
    setValidName(USER_REGEX.test(user));
  }, [user]);

  useEffect(() => {
    setValidEmail(EMAIL_REGEX.test(email));
  }, [email]);

  useEffect(() => {
    const result = PWD_REGEX.test(pwd);
    setValidPwd(result);
    setValidMatch(pwd === matchPwd);
  }, [pwd, matchPwd]);

  useEffect(() => {
    setErrMsg("");
  }, [user, pwd, matchPwd]);

  const handleSubmit = async (e: SyntheticEvent<HTMLFormElement>) => {
    e.preventDefault();
    // if button enabled with JS hack
    const v1 = USER_REGEX.test(user);
    const v2 = EMAIL_REGEX.test(email);
    const v3 = PWD_REGEX.test(pwd);
    if (!v1 || !v2 || !v3) {
      setErrMsg("Invalid Entry");
      return;
    }
    try {
      await api.post("/auth/register", {
        Username: user,
        Email: email,
        Password: pwd,
      });
      setUser("");
      setEmail("");
      setPwd("");
      setMatchPwd("");
      setSuccess(true);
    } catch (err) {
      if (axios.isAxiosError(err)) {
        if (!err?.response) {
          setErrMsg("No Server Response");
        } else if (err.response?.status === 400) {
          setErrMsg("User with username or email already exists");
        } else {
          setErrMsg("Registration Failed");
        }
        setSuccess(false);
      } else console.error(err);
    }
  };

  return (
    <div className="register-body">
      {success ? (
        <div className="wrapper-success">
          <h1>Success!</h1>
          <Link to="/login" className="btn btn-primary">
            Go to login page
          </Link>
        </div>
      ) : (
        <div className="wrapper">
          {errMsg && (
            <p className="errMsg" aria-live="assertive">
              {errMsg}
            </p>
          )}
          <h1>Register</h1>
          <form onSubmit={handleSubmit} action="">
            <label htmlFor="username">
              Username
              <span className={validName ? "valid" : "hide"}>
                <FontAwesomeIcon icon={faCheck} />
              </span>
              <span className={validName || !user ? "hide" : "invalid"}>
                <FontAwesomeIcon icon={faTimes} />
              </span>
            </label>
            <div className="input-box">
              <input
                type="text"
                id="username"
                ref={userRef}
                autoComplete="off"
                value={user}
                onChange={(e) => setUser(e.target.value)}
                aria-invalid={validName ? "false" : "true"}
                aria-describedby="uidnote"
                onFocus={() => setUserFocus(true)}
                onBlur={() => setUserFocus(false)}
                required
              />
              <FaUser className="icon" />
            </div>
            {userFocus && user && !validName && (
              <p id="uidnote" className="instructions">
                <FontAwesomeIcon icon={faInfoCircle} />
                4 to 24 characters. <br />
                Must begin with a letter.
                <br />
                Letters, numbers, underscores allowed.
              </p>
            )}
            <label htmlFor="email">
              Email
              <span className={validEmail ? "valid" : "hide"}>
                <FontAwesomeIcon icon={faCheck} />
              </span>
              <span className={validEmail || !email ? "hide" : "invalid"}>
                <FontAwesomeIcon icon={faTimes} />
              </span>
            </label>
            <div className="input-box">
              <input
                type="text"
                id="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                aria-invalid={validEmail ? "false" : "true"}
                aria-describedby="emailnote"
                onFocus={() => setEmailFocus(true)}
                onBlur={() => setEmailFocus(false)}
                required
              />
              <FaEnvelope className="icon" />
            </div>
            {emailFocus && email && !validEmail && (
              <p id="emailnote" className="instructions">
                <FontAwesomeIcon icon={faInfoCircle} />
                Must provide an valid email. It should be lowercase. <br />
                An example of a valid email is: "mail@example.com"
              </p>
            )}
            <label htmlFor="password">
              Password
              <span className={validPwd ? "valid" : "hide"}>
                <FontAwesomeIcon icon={faCheck} />
              </span>
              <span className={validPwd || !pwd ? "hide" : "invalid"}>
                <FontAwesomeIcon icon={faTimes} />
              </span>
            </label>
            <div className="input-box">
              <input
                type="password"
                id="password"
                value={pwd}
                onChange={(e) => setPwd(e.target.value)}
                aria-invalid={validPwd ? "false" : "true"}
                aria-describedby="pwdnote"
                onFocus={() => setPwdFocus(true)}
                onBlur={() => setPwdFocus(false)}
                required
              />
              <FaLock className="icon" />
            </div>
            {pwdFocus && !validPwd && (
              <p id="pwdnote" className="instructions">
                <FontAwesomeIcon icon={faInfoCircle} />
                8 to 24 characters. <br />
                Must include uppercase and lowercase letters, a number and a
                special character.
                <br />
                Allowed special characters {" "}
                <span aria-label="exclamation mark">!</span>
                <span aria-label="at symbol">@</span>
                <span aria-label="hastag">#</span>
                <span aria-label="dollar sign">$</span>
                <span aria-label="percent">%</span>
                <br />
                No whitespaces allowed.
              </p>
            )}
            <label htmlFor="confirm_pwd">
              Confirm password
              <span className={validMatch && matchPwd ? "valid" : "hide"}>
                <FontAwesomeIcon icon={faCheck} />
              </span>
              <span className={validMatch || !matchPwd ? "hide" : "invalid"}>
                <FontAwesomeIcon icon={faTimes} />
              </span>
            </label>
            <div className="input-box">
              <input
                type="password"
                id="confirm_pwd"
                value={matchPwd}
                onChange={(e) => setMatchPwd(e.target.value)}
                aria-invalid={validMatch ? "false" : "true"}
                aria-describedby="confirmnote"
                onFocus={() => setMatchFocus(true)}
                onBlur={() => setMatchFocus(false)}
                required
              />
              <FaLock className="icon" />
            </div>
            {matchFocus && !validMatch && (
              <p id="confirmnote" className="instructions">
                <FontAwesomeIcon icon={faInfoCircle} />
                Must match the first password input field.
              </p>
            )}

            <button
              disabled={
                !validName || !validEmail || !validPwd || !validMatch
                  ? true
                  : false
              }
              type="submit"
            >
              Register
            </button>
            <div className="register-link">
              <p>
                Already have an account? <Link to="/login">Login</Link>
              </p>
            </div>
          </form>
        </div>
      )}
    </div>
  );
};

export default RegisterPage;
