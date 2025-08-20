import React, { type ReactNode } from "react";
import { useAuthProvider } from "@hooks/features/auth";

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const { AuthContext, value } = useAuthProvider();

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};
