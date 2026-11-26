import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { tap } from 'rxjs';

interface LoginResponse {
  token: string;
  expires: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5077/api';
  private readonly tokenKey = 'token';

  isLoggedIn = signal(!!sessionStorage.getItem(this.tokenKey));

  get token(): string | null {
    return sessionStorage.getItem(this.tokenKey);
  }

  login(email: string, password: string) {
    return this.http
      .post<LoginResponse>(`${this.baseUrl}/auth/login`, { email, password })
      .pipe(
        tap((res) => {
          sessionStorage.setItem(this.tokenKey, res.token);
          this.isLoggedIn.set(true);
        })
      );
  }

  logout() {
    sessionStorage.removeItem(this.tokenKey);
    this.isLoggedIn.set(false);
  }
}