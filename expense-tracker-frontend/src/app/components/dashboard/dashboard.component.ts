import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Category } from '../../models/category.model';
import { CategoryService } from '../../services/category.service';
import { Expense } from '../../models/expense.model';
import { ExpenseService } from '../../services/expense.service';
import {
  Chart,
  ArcElement,
  Tooltip,
  Legend,
  Title,
  CategoryScale,
  LinearScale,
  PieController,
} from 'chart.js';
Chart.register(
  ArcElement,
  Tooltip,
  Legend,
  Title,
  CategoryScale,
  LinearScale,
  PieController
);

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  imports: [FormsModule, CommonModule, RouterOutlet],
  standalone: true,
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  username: string = '';
  maxMonthlyExpenses: number = 0;
  currentMonthlyExpenses: number = 0;
  expensesPercentage: number = 0;
  dropdownVisible: boolean = false;
  currentDate: string = '';
  daysLeft: number = 0;
  categories: Category[] = [];
  userId: string = '';
  selectedCategoryId: string = '';
  amount: number = 0;
  description: string = '';
  expenses: Expense[] = [];
  chartInstance: Chart | null = null;
  showModal: boolean = false; // Tracks whether the modal is shown or not
  selectedExpense: Expense | null = null; // Stores the selected expense to display

  constructor(
    private authService: AuthService,
    private router: Router,
    private categoryService: CategoryService,
    private expenseService: ExpenseService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadUserData();
    this.setCurrentDateInfo();
    this.loadCategories();
    this.loadExpenses();
  }

  loadExpenses(): void {
    this.expenseService.getExpenses(this.userId).subscribe(
      (data: Expense[]) => {
        this.expenses = data;
        this.updateChart();
      },
      (error) => {
        console.error('Error fetching expenses:', error);
      }
    );
  }

  loadCategories(): void {
    if (this.userId) {
      this.categoryService
        .getCategories(this.userId)
        .subscribe((categories: Category[]) => {
          this.categories = categories;
          this.updateChart();
        });
    }
  }
  // Load user data from localStorage
  loadUserData(): void {
    if (this.authService.isAuthenticated()) {
      this.username = localStorage.getItem('username') || '';
      this.userId = localStorage.getItem('userId') || '';
      this.maxMonthlyExpenses =
        Number(localStorage.getItem('maxMonthlyExpenses')) || 0;
      this.currentMonthlyExpenses =
        Number(localStorage.getItem('currentMonthlyExpenses')) || 0;
      this.expensesPercentage =
        (this.currentMonthlyExpenses / this.maxMonthlyExpenses) * 100;
    } else {
      this.router.navigate(['/login']); 
    }
  }

  toggleDropdown() {
    this.dropdownVisible = !this.dropdownVisible; // Toggles visibility on button click
  }

  setCurrentDateInfo(): void {
    const today = new Date();
    const endOfMonth = new Date(today.getFullYear(), today.getMonth() + 1, 0); // Get last day of current month
    const remainingDays = Math.ceil(
      (endOfMonth.getTime() - today.getTime()) / (1000 * 3600 * 24)
    ); // Calculate remaining days

    // Set the current date in a readable format (e.g., "December 6, 2024")
    this.currentDate = today.toLocaleDateString('en-US', {
      weekday: 'long',
      month: 'long',
      day: 'numeric',
      year: 'numeric',
    });

    // Set the number of days left in the month
    this.daysLeft = remainingDays;
  }

  // Update the maxMonthlyExpenses in the localStorage
  updateMaxExpenses(newMaxExpenses: number): void {
    this.authService.updateMaxExpenses(newMaxExpenses).subscribe(
      (response) => {
        console.log(response);
        this.authService.updateUserData(
          'maxMonthlyExpenses',
          newMaxExpenses.toString()
        );
        this.maxMonthlyExpenses = newMaxExpenses;
        this.expensesPercentage =
          (this.currentMonthlyExpenses / this.maxMonthlyExpenses) * 100;
      },
      (error: any) => {}
    );
  }

  addExpense(): void {
    if (this.amount && this.selectedCategoryId && this.description) {
      const newExpense = {
        amount: this.amount,
        description: this.description,
        categoryId: this.selectedCategoryId,
        date: new Date().toISOString(),
      };
      this.expenseService
        .addExpense(this.userId, this.selectedCategoryId, newExpense)
        .subscribe(
          (expense) => {
            console.log('Expense added successfully:', expense);
            this.expenses.push(expense);
            // Manually trigger change detection
            this.cdr.detectChanges();
            this.updateChart();
            this.currentMonthlyExpenses =
              this.currentMonthlyExpenses + this.amount;
            this.expensesPercentage =
              (this.currentMonthlyExpenses / this.maxMonthlyExpenses) * 100;
            if (this.expensesPercentage >= 100) {
              alert('You have exceeded your monthly expense limit!');
            }
            this.authService.updateUserData(
              'currentMonthlyExpenses',
              this.currentMonthlyExpenses.toString()
            );
            this.selectedCategoryId = '';
            this.amount = 0;
            this.description = '';
          },
          (error) => {
            console.error('Error adding expense:', error);
          }
        );
    } else {
      alert('Please fill out all fields.');
    }
  }

  // Logout function
  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']); // Redirect to login page after logout
  }

  // View expense details (example functionality)
  viewExpense(expense: Expense): void {
    console.log('Viewing expense:', expense);
    this.selectedExpense = expense; // Store the selected expense
    this.showModal = true; // Show the modal
  }

  // Delete an expense
  deleteExpense(expense: Expense): void {
    if (confirm('Are you sure you want to delete this expense?')) {
      this.expenseService.deleteExpense(expense?.id).subscribe({
        next: () => {
          this.expenses = this.expenses.filter((e) => e.id !== expense.id);
          this.cdr.detectChanges();
          this.currentMonthlyExpenses =
            this.currentMonthlyExpenses - expense.amount;
          this.expensesPercentage =
            (this.currentMonthlyExpenses / this.maxMonthlyExpenses) * 100;
          this.authService.updateUserData(
            'currentMonthlyExpenses',
            this.currentMonthlyExpenses.toString()
          );
          this.updateChart(); // Update the chart after deletion
        },
        error: (err) => {
          console.error('Error deleting expense:', err);
        },
      });
    }
  }

  // Function to get the category name by categoryId
  getCategoryName(categoryId: string): string {
    const category = this.categories.find((c) => c.id === categoryId);
    return category ? category.name : this.selectedCategoryId;
  }

  //chart
  updateChart(): void {
    const canvas = document.getElementById('pieChart') as HTMLCanvasElement;

    // Check if the chart already exists and destroy it
    if (Chart.getChart(canvas)) {
      Chart.getChart(canvas)?.destroy();
    }

    if (this.expenses.length > 0 && this.categories.length > 0) {
      // Group expenses by categoryId
      const categoryExpenses = this.categories.map((category) => {
        const totalAmount = this.expenses
          .filter((expense) => expense.categoryId === category.id)
          .reduce((sum, expense) => sum + expense.amount, 0);
        return { category: category.name, totalAmount };
      });

      // Data for the pie chart
      const chartData = {
        labels: categoryExpenses.map((exp) => exp.category),
        datasets: [
          {
            data: categoryExpenses.map((exp) => exp.totalAmount),
            backgroundColor: [
              '#FF5733',
              '#33FF57',
              '#3357FF',
              '#FFC300',
              '#FF33A1',
            ],
          },
        ],
      };

      // Create or update the pie chart
      const ctx = (
        document.getElementById('pieChart') as HTMLCanvasElement
      ).getContext('2d');
      if (ctx) {
        new Chart(ctx, {
          type: 'pie',
          data: chartData,
          options: {
            responsive: true,
            plugins: {
              legend: {
                position: 'top',
              },
              tooltip: {
                callbacks: {
                  label: (context) => {
                    const value = context.raw;
                    return `${context.label}: $${value}`;
                  },
                },
              },
            },
          },
        });
      }
    }
  }

  closeModal(): void {
    this.showModal = false;
    this.selectedExpense = null;
  }
}
