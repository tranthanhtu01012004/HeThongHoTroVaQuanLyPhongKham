import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginService } from '../../../services/login/login.service';
import { LoginStore } from '../../../store/LoginStore';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandlerService } from '../../../commons/ErrorHandlerService';
import { ILoginInformation } from '../../../interfaces/Auth/ILoginInformation';
import { Router } from '@angular/router';
import { NotificationComponent } from '../notification/notification.component';
import { LoginData } from '../../../responses/LoginData';
import { ApiResponse } from '../../../commons/ApiResponse';
import { AuthService } from '../../../services/Auth/AuthService';

@Component({
  selector: 'app-dang-nhap',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule,
    NotificationComponent
],
  templateUrl: './dang-nhap.component.html',
  styleUrl: './dang-nhap.component.css'
})
export class DangNhapComponent {
  loginForm: FormGroup;
  showNotification: boolean = false;
  errorMessages: string[] = [];

  constructor(
    private fb: FormBuilder,
    private loginService: LoginService,
    private loginStore: LoginStore,
    private errorHandler: ErrorHandlerService,
    private router: Router,
    private authService: AuthService
  ) {
    this.loginForm = this.fb.group({
      tenDangNhap: ['', Validators.required],
      matKhau: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      const loginInfor: ILoginInformation = this.loginForm.value;

      this.loginService.login(loginInfor).subscribe({
        next: (response) => {
          this.handleLoginResponse(response);
        },
        error: (err: HttpErrorResponse) => {
          this.handleError(err);
        }
      });
    } else {
      this.showFormValidationErrors();
    }
  }
  
  private handleLoginResponse(response: ApiResponse<LoginData>): void {
    if (response.status && response.data) {
      // Set token
      this.authService.setToken(response.data.token);
      console.log('Token set thành công cho localStorage:', response.data.token);

      this.loginStore.setAuthenticated(true);
      this.clearNotifications();
      console.log('Login successful:', response);
      this.router.navigate(['/dich-vu']);
    } else {
      this.errorMessages = [response.message || 'Đăng nhập thất bại'];
      this.showNotification = true;
      console.log('Login failed:', response.message);
    }
  }
  private handleError(error: HttpErrorResponse): void {
    this.errorMessages = this.errorHandler.handleError(error);
    this.showNotification = true;
    console.error('Login error:', error);
  }

  private showFormValidationErrors(): void {
    this.errorMessages = ['Vui lòng điền đầy đủ thông tin đăng nhập'];
    this.showNotification = true;
  }

  private clearNotifications(): void {
    this.showNotification = false;
    this.errorMessages = [];
  }

  closeNotification(): void {
    this.clearNotifications();
  }
}
