import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  username: string = '';
  maxMonthlyExpenses: number = 0;
  currentMonthlyExpenses: number = 0;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    // Check if user is authenticated when the dashboard is initialized
    this.loadUserData();
  }

  loadUserData(): void {
    if (this.authService.isAuthenticated()) {
      this.username = localStorage.getItem('username') || '';
      this.maxMonthlyExpenses = Number(localStorage.getItem('maxMonthlyExpenses')) || 0;
      this.currentMonthlyExpenses = Number(localStorage.getItem('currentMonthlyExpenses')) || 0;
    } else {
      this.router.navigate(['/login']); // Redirect to login if user is not authenticated
    }
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']); // Redirect to login page after logout
  }
}
