import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { RegisterRequest, User } from '../../models/user.model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  standalone: true,
  imports: [FormsModule,CommonModule,RouterOutlet],  // Add FormsModule and RouterOutlet to imports
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  username: string = '';
  password: string = '';
  maxMonthlyExpenses: number = 1000;
  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  register(): void {
    const registerRequest: RegisterRequest = { username: this.username, password: this.password, maxMonthlyExpenses: this.maxMonthlyExpenses };

    this.authService.register(registerRequest).subscribe(
      (response: { user: User; }) => { 
        
        this.authService.setUserData(response.user);
        // Navigate to the home page or dashboard
        this.router.navigate(['/dashboard']);
      },
      (error) => {
        this.errorMessage = 'Registration failed. Please try again.';
      }
    );
  }
}
