import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/Auth/AuthService';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Kiểm tra trạng thái đăng nhập
  if (authService.isAuthenticated()) {
    return true;
  } else {
    router.navigate(['/dang-nhap'], {
      queryParams: { returnUrl: state.url },
      replaceUrl: true
    });
    return false;
  }
};