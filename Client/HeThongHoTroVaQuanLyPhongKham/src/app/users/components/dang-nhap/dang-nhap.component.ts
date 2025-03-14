import { Component, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginService } from '../../../services/login/login.service';
import { LoginStore } from '../../../store/LoginStore';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ILoginInformation } from '../../../interfaces/Auth/ILoginInformation';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationComponent } from '../notification/notification.component';
import { ILogin } from '../../../interfaces/login/ILogin';
import { ApiResponse } from '../../../commons/ApiResponse';
import { AuthService } from '../../../services/Auth/AuthService';
import { RegisterService } from '../../../services/register/register.service';
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
  isLoading: boolean = false;
  submitted: boolean = false;

  constructor(
    private fb: FormBuilder,
    private loginService: LoginService,
    private loginStore: LoginStore,
    private router: Router,
    private authService: AuthService,
    public notificationService: ErrorNotificationService,
    private route: ActivatedRoute,
    private registerService: RegisterService
  ) {
    this.loginForm = this.fb.group({
      tenDangNhap: ['', Validators.required],
      matKhau: ['', Validators.required]
    });
  }

  // Getter để truy cập dễ dàng vào các form controls
  get f() {
    return this.loginForm.controls;
  }

  onLogin(): void {
    this.submitted = true; // Đánh dấu form đã được submit

    if (this.loginForm.valid) {
      this.isLoading = true;
      const loginInfo: ILoginInformation = this.loginForm.value;

      this.loginService.login(loginInfo).subscribe({
        next: (response) => {
          this.handleLoginResponse(response);
          this.isLoading = false;
        },
        error: (err: HttpErrorResponse) => {
          this.notificationService.handleError(err);
          this.isLoading = false;
        }
      });
    }
  }

  onRegister(): void {
    this.submitted = true; // Đánh dấu form đã được submit

    if (this.loginForm.valid) {
      this.isLoading = true;
      const registerInfo: ILoginInformation = this.loginForm.value;

      this.registerService.register(registerInfo).subscribe({
        next: (response) => {
          this.handleRegisterResponse(response);
          this.isLoading = false;
        },
        error: (err: HttpErrorResponse) => {
          this.notificationService.handleError(err);
          this.isLoading = false;
        }
      });
    }
  }

  private handleLoginResponse(response: ApiResponse<ILogin>): void {
    if (response.status && response.data) {
      // Set token
      this.authService.setToken(response.data.token);
      console.log('Token và role đã được lưu vào localStorage:', response.data);

      
      // Cập nhật trạng thái đăng nhập trong LoginStore
      this.loginStore.setAuthenticated(true);

      const role = this.authService.getRoleFromToken();
      if (role)
        this.loginStore.setRole(role);
      else {
        console.warn('Không tìm thấy role trong token');
        this.notificationService.showError('Không tìm thấy vai trò trong token.');
        return;
      }

      this.notificationService.clearNotifications();
      console.log('Login successful:', response);

      const returnUrl = this.route.snapshot.queryParams['returnUrl'];
      if (returnUrl) {
        this.router.navigateByUrl(returnUrl);
      } else {
        if (role !== 'BenhNhan') {
          this.router.navigate(['/admin/dashboard']);
        } else {
          this.router.navigate(['/dich-vu']);
        }
      }
    } else {
      this.notificationService.showError(response.message || 'Đăng nhập thất bại');
    }
  }

  private handleRegisterResponse(response: ApiResponse<ILogin>): void {
    if (response.status && response.data) {
      this.notificationService.showSuccess('Đăng ký thành công! Vui lòng đăng nhập để tiếp tục.');
      this.loginForm.reset();
      this.submitted = false; // Reset trạng thái submitted sau khi đăng ký thành công
    } else {
      this.notificationService.showError(response.message || 'Đăng ký thất bại');
    }
  }
}
