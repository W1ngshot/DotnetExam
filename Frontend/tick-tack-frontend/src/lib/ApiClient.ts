import ky from 'ky';
import { LoginResponseDto } from './LoginResponseDto';

class TokenManager {
  private token?: string | null = null;

  constructor() {
    this.token = localStorage.getItem('token');
  }

  public get Token() {
    return this.token;
  }

  public UpsertFromDto(dto?: LoginResponseDto) {
    this.token = dto?.token;
    this.lsSetOrRemove("token", dto?.token);
  }

  private lsSetOrRemove(key: string, value?: string | null) {
    if (value == null)
      localStorage.removeItem(key);
    else
      localStorage.setItem(key, value);
  }
}


export const apiClient = ky.extend({
  prefixUrl: import.meta.env.VITE_API_URL || undefined,
  hooks: {
    beforeRequest: [
      request => {
        if(tokenStore.Token)
          request.headers.set('Authorization', `Bearer ${tokenStore.Token}`);
        // if(realtimeConnection.connection.connectionId)
        //   request.headers.set('Realtime-Connection', realtimeConnection.connection.connectionId);
      }
    ]
  }
});

export const tokenStore = new TokenManager();