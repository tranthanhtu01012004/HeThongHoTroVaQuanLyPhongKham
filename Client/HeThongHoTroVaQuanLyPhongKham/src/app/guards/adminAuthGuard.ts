import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/Auth/AuthService';
import { LoginStore } from '../store/LoginStore';
import { map, take } from 'rxjs/operators';

// tltk: https://v18.angular.dev/api/router/CanActivateFn?tab=description
// CanActivateFn để bảo vệ các route có tiền tố /admin/**
export const adminAuthGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const loginStore = inject(LoginStore);
  const router = inject(Router);

  // Kiểm tra trạng thái đăng nhập từ AuthService
  const isAuthenticated = authService.isAuthenticated();

  if (isAuthenticated) {
    const role = authService.getRoleFromToken();
    const allowedRoles = [
      'QuanLy',
      'BacSi',
      'LeTan',
      'NhanVienHanhChinh',
      'DuocSi',
      'KeToan',
      'kyThuatVienXetNghiem',
      'TroLyBacSy',
      'YTa'
    ];

    const normalizedRole = role?.toLowerCase();
    const normalizedAllowedRoles = allowedRoles.map(r => r.toLowerCase());

    if (normalizedRole && normalizedAllowedRoles.includes(normalizedRole)) {
      return true;
    } else {
      const returnUrl = state.url; // Lấy URL hiện tại từ state
      if (returnUrl && returnUrl.startsWith('/admin')) {
        router.navigate(['/unauthorized'], {
          queryParams: { returnUrl },
          replaceUrl: true
        });
      } else {
        if (role?.toLowerCase() !== 'benhnhan') {
          router.navigate(['/admin/dashboard']);
        } else {
          router.navigate(['/dich-vu-y-te']);
        }
      }
      return false;
    }
  } else {
    router.navigate(['/dang-nhap'], {
      queryParams: { returnUrl: state.url },
      replaceUrl: true
    });
    return false;
  }
};