export interface LoginDto {
  username: string;
  password: string;
}

export interface LoginResult {
  success: boolean;
  error: string;
  token: string;
  expiration: string;
  username: string;
  role: string;
}

export interface UserInfoDto {
  userId: string;
  username: string;
  role: string;
}
