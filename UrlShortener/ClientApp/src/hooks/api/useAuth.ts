import { authApi } from "@api/endpoints/auth";
import type { LoginDto } from "@api-types/auth";

export const useAuthApi = () => {
  const login = async (loginDto: LoginDto) => {
    return await authApi.login(loginDto);
  };

  const getCurrentUser = async () => {
    return await authApi.getCurrentUser();
  };

  const logout = async () => {
    return await authApi.logout();
  };

  return {
    login,
    getCurrentUser,
    logout,
  };
};
