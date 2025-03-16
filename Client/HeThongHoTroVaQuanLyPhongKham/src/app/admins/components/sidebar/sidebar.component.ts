import { Component, ViewEncapsulation } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/Auth/AuthService';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, RouterLink, NgClass],
  templateUrl: './sidebar.component.html',
  styleUrls: [
    './sidebar.component.css',
    '/public/assets/admins/css/styles.css'
  ],
  encapsulation: ViewEncapsulation.None
})
export class SidebarComponent {
  role: string | null = null; // Biến để hiển thị vai trò

  constructor(
    private authService: AuthService, // Tiêm AuthService
    private router: Router
  ) {
    this.loadUserRole(); // Tải vai trò khi khởi tạo
  }

  // Tải vai trò người dùng
  private loadUserRole(): void {
    this.role = this.authService.getRoleFromToken() || 'Unknown';
  }

  // Phương thức kiểm tra quyền
  hasPermission(requiredRole: string): boolean {
    const userRole = this.authService.getRoleFromToken();
    return userRole === requiredRole;
  }

  logout(): void {
    this.authService.removeToken(); // Xóa token sử dụng AuthService
    this.router.navigate(['/login']); // Điều hướng về trang login
  }
}
