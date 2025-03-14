import { Component, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginService } from '../../../services/login/login.service';
import { LoginStore } from '../../../store/LoginStore';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ILoginInformation } from '../../../interfaces/Auth/ILoginInformation';
import { Router } from '@angular/router';
import { NotificationComponent } from '../notification/notification.component';
import { ILogin } from '../../../interfaces/login/ILogin';
import { ApiResponse } from '../../../commons/ApiResponse';
import { AuthService } from '../../../services/Auth/AuthService';
import { ErrorNotificationService } from '../../../services/handle-error/ErrorNotificationService';

@Component({
  selector: 'app-dang-nhap',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule,
    NotificationComponent
],
  templateUrl: './dang-nhap.component.html',
  styleUrls: [
    './dang-nhap.component.css',
    "/public/assets/users/bootstrap/owl.carousel.min.css",
    "/public/assets/users/bootstrap/tempusdominus-bootstrap-4.min.css",
    "/public/assets/users/bootstrap/bootstrap.min.css",
    "/public/assets/users/css/style.css"
  ],
  encapsulation: ViewEncapsulation.None
})
export class DangNhapComponent {
  loginForm: FormGroup;
  showNotification: boolean = false;
  errorMessages: string[] = [];

  constructor(
    private fb: FormBuilder,
    private loginService: LoginService,
    private loginStore: LoginStore,
    private router: Router,
    private authService: AuthService,
    public errorNotificationService: ErrorNotificationService
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
          this.errorNotificationService.handleError(err);
        }
      });
    } else {
      this.errorNotificationService.showFormValidationErrors();
    }
  }
  
  private handleLoginResponse(response: ApiResponse<ILogin>): void {
    if (response.status && response.data) {
      // Set token
      this.authService.setToken(response.data.token);
      console.log('Token set thành công cho localStorage:', response.data.token);

      this.loginStore.setAuthenticated(true);
      this.errorNotificationService.clearNotifications();
      console.log('Login successful:', response);
      this.router.navigate(['/dich-vu']);
    } else {
      this.errorMessages = [response.message || 'Đăng nhập thất bại'];
      this.showNotification = true;
      console.log('Login failed:', response.message);
    }
  }
}
