import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { LoginRequest, User } from '../../models/user.model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [FormsModule,CommonModule,RouterOutlet],
  standalone: true
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  login(): void {
    const loginRequest: LoginRequest = { username: this.username, password: this.password };

    this.authService.login(loginRequest).subscribe(
      (response: { user: User; }) => {
        // Store user data after successful login
        this.authService.setUserData(response.user);
        // Navigate to the home page or dashboard
        this.router.navigate(['/dashboard']);
      },
      (error: any) => {
        this.errorMessage = 'Login failed. Please check your credentials.';
      }
    );
  }
}
