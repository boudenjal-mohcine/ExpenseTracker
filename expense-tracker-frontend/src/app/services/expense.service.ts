// src/app/services/expense.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Expense } from '../models/expense.model';
import { ApiConfig } from '../api/api.config'; // Import ApiConfig

@Injectable({
  providedIn: 'root'
})
export class ExpenseService {

  private readonly baseUrl = ApiConfig.baseUrl;

  constructor(private http: HttpClient) { }

  // Function to fetch categories for a specific user
  getExpenses(userId: string): Observable<Expense[]> {
    return this.http.get<Expense[]>(`${this.baseUrl}/expenses/user/${userId}`);
  }

  addExpense(userId: string, categoryId: string, expense: Expense): Observable<Expense> {
    return this.http.post<Expense>(`${this.baseUrl}/expenses/${userId}/${categoryId}`, expense);
  }
}
