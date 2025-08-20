import type { AxiosResponse } from "axios";
import type { LoginDto, LoginResult, UserInfoDto } from "@api-types/auth";
import { httpClient } from "api/client";

export const authApi = {
  BASE_URL: "/auth",

  async login(loginDto: LoginDto): Promise<LoginResult> {
    const response: AxiosResponse<LoginResult> = await httpClient.post(
      `${this.BASE_URL}/login`,
      loginDto
    );
    return response.data;
  },

  async getCurrentUser(): Promise<UserInfoDto> {
    const response: AxiosResponse<UserInfoDto> = await httpClient.get(
      `${this.BASE_URL}/me`
    );
    return response.data;
  },

  async logout(): Promise<string> {
    const response: AxiosResponse<string> = await httpClient.post(
      `${this.BASE_URL}/logout`
    );
    return response.data;
  },
};
