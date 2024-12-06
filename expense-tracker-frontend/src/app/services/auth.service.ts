import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiConfig } from '../api/api.config'; // Import ApiConfig
import { Observable } from 'rxjs';
import { LoginRequest, RegisterRequest, LoginResponse, User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly baseUrl = ApiConfig.baseUrl;

  constructor(private http: HttpClient) {}

  login(request: LoginRequest): Observable<LoginResponse> {
    const url = `${this.baseUrl}/users/login`;
    return this.http.post<LoginResponse>(url, request);
  }

  register(request: RegisterRequest): Observable<LoginResponse> {
    const url = `${this.baseUrl}/users/register`;
    return this.http.post<LoginResponse>(url, request);
  }

  // Check if user is authenticated by verifying if `userId` exists in localStorage
  isAuthenticated(): boolean {
    return localStorage.getItem('userId') !== null;
  }

  // Set user data in localStorage after successful login or registration
  setUserData(user: User): void {
    localStorage.setItem('userId', user.id);
    localStorage.setItem('username', user.username);
    localStorage.setItem('maxMonthlyExpenses', user.maxMonthlyExpenses.toString());
    localStorage.setItem('currentMonthlyExpenses', user.currentMonthlyExpenses.toString());
  }

  // Clear user data from localStorage (for logout)
  logout(): void {
    localStorage.removeItem('userId');
    localStorage.removeItem('username');
    localStorage.removeItem('maxMonthlyExpenses');
    localStorage.removeItem('currentMonthlyExpenses');
  }
}
