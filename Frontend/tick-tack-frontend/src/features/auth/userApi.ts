import { apiClient } from "@/lib/ApiClient";

export interface MeUserInfo{
  userId: string;
  username: string;
  rating: number;
}

export async function getMyUser(){
  return apiClient.get('auth/profile').json<MeUserInfo | string>();
}

export interface LoginRequestDto{
  login: string;
  password: string;
}

export interface LoginResponseDto{
  token: string;
}

export function login(request: LoginRequestDto){
  return apiClient.post('auth/login', { json: request }).json<LoginResponseDto>();
}

export interface RegisterRequestDto {
  login: string;
  password: string;
}

export interface RegisterResponseDto {
  token: string;
}

export function register(request: RegisterRequestDto){
  return apiClient.post('auth/register', { json: request }).json<RegisterResponseDto>();
}