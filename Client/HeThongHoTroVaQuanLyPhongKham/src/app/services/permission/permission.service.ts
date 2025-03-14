import { Injectable } from '@angular/core';
import { AuthService } from '../Auth/AuthService';

@Injectable({
  providedIn: 'root'
})
export class PermissionService {
  constructor(private authService: AuthService) {}

  // Kiểm tra vai trò hiện tại của người dùng
  private getUserRole(): string | null {
    return this.authService.getRoleFromToken();
  }

  // Kiểm tra xem người dùng có vai trò cụ thể không
  hasRole(role: string): boolean {
    return this.getUserRole() === role;
  }

  // Kiểm tra xem người dùng có bất kỳ vai trò nào trong danh sách không
  hasAnyRole(roles: string[]): boolean {
    const userRole = this.getUserRole();
    return userRole ? roles.includes(userRole) : false;
  }
}
