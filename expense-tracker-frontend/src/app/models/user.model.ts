export interface User {
    id: string;
    username: string;
    maxMonthlyExpenses: number;
    currentMonthlyExpenses: number;
  }
  
  export interface LoginResponse {
    message: string;
    user: User;
  }
  
  export interface LoginRequest {
    username: string;
    password: string;
  }
  
  export interface RegisterRequest {
    username: string;
    password: string;
    maxMonthlyExpenses: number;
  }
  