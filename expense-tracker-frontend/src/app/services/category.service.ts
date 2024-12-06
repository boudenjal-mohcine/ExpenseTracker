// src/app/services/category.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Category } from '../models/category.model';
import { ApiConfig } from '../api/api.config'; // Import ApiConfig

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  private readonly baseUrl = ApiConfig.baseUrl;

  constructor(private http: HttpClient) { }

  // Function to fetch categories for a specific user
  getCategories(userId: string): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.baseUrl}/categories/${userId}`);
  }
}
