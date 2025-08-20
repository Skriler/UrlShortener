import React from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuthContext } from "@hooks/features/auth";

export const Navbar: React.FC = () => {
  const { isAuthenticated, user, logout } = useAuthContext();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/");
  };

  return (
    <nav>
      <div>
        <Link to="/">URL Shortener</Link>
      </div>
      <div>
        {isAuthenticated ? (
          <div>
            <span>
              Welcome, {user?.username} ({user?.role})
            </span>
            <button onClick={handleLogout}>Logout</button>
          </div>
        ) : (
          <Link to="/login">Login</Link>
        )}
      </div>
    </nav>
  );
};
