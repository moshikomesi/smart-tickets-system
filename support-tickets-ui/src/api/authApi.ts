import type { LoginRequest, LoginResponse } from "../types/auth";
import { httpPost } from "./httpClient";

export const authApi = {
  login: (username: string, password: string) =>
    httpPost<LoginResponse, LoginRequest>("/api/auth/login", {
      username,
      password,
    }),

  guest: () =>
    httpPost<LoginResponse, void>("/api/auth/guest", undefined),
};
