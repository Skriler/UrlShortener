import { createContext, useContext, useEffect, useState } from "react";
import { getToken, setToken, removeToken } from "@api/client";
import type { UserInfoDto } from "@api-types/auth";
import { useAuthApi } from "@hooks/api/useAuth";

interface AuthContextType {
  user: UserInfoDto | null;
  isAuthenticated: boolean;
  isAdmin: boolean;
  login: (
    username: string,
    password: string
  ) => Promise<{ success: boolean; error?: string }>;
  logout: () => void;
  loading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuthContext = (): AuthContextType => {
  const context = useContext(AuthContext);

  if (context === undefined) {
    throw new Error("useAuthContext must be used within an AuthProvider");
  }

  return context;
};

export const useAuthProvider = () => {
  const [user, setUser] = useState<UserInfoDto | null>(null);
  const [loading, setLoading] = useState(true);
  const authApi = useAuthApi();

  useEffect(() => {
    const initializeAuth = async () => {
      const token = getToken();

      if (token) {
        try {
          const userInfo = await authApi.getCurrentUser();

          setUser(userInfo);
        } catch {
          removeToken();
        }
      }

      setLoading(false);
    };

    initializeAuth();
  }, [authApi]);

  const login = async (
    username: string,
    password: string
  ): Promise<{ success: boolean; error?: string }> => {
    try {
      const result = await authApi.login({ username, password });

      if (result.success) {
        setToken(result.token);

        const userInfo = await authApi.getCurrentUser();
        setUser(userInfo);

        return { success: true };
      } else {
        return { success: false, error: result.error };
      }
    } catch {
      return { success: false, error: "Login failed. Please try again." };
    }
  };

  const logout = () => {
    authApi.logout().catch(() => {});
    removeToken();
    setUser(null);
  };

  const isAuthenticated = !!user;
  const isAdmin = user?.role === "Admin";

  return {
    AuthContext,
    value: {
      user,
      isAuthenticated,
      isAdmin,
      login,
      logout,
      loading,
    },
  };
};
