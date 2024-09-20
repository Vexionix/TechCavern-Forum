import api from './api';
import { useAuth } from '../contexts/AuthContext';

const logout = async () => {
    const { setToken, setUserId, setRole } = useAuth();
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    localStorage.removeItem('role');
    setToken(null);
    setUserId(null)
    setRole(null);

    try {
        await api.get("/auth/logout", {});
      } catch (err) {
        console.error(err);
      }
    }

  export default logout;