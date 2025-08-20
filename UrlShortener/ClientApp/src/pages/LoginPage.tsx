import React from "react";
import { Navigate } from "react-router-dom";
import { useAuthContext, useLoginLogic } from "@hooks/features/auth";
import { LoginForm } from "@components/Auth";

const LoginPage: React.FC = () => {
  const { isAuthenticated } = useAuthContext();
  const loginLogic = useLoginLogic();

  if (isAuthenticated) {
    return <Navigate to="/" replace />;
  }

  return (
    <div>
      <h2>Login</h2>
      <LoginForm {...loginLogic} />
    </div>
  );
};

export default LoginPage;
