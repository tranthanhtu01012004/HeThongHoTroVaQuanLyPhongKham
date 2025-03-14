import { isPlatformBrowser } from "@angular/common";
import { Inject, Injectable, PLATFORM_ID } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private readonly TOKEN_KEY = 'auth_token';

    constructor(@Inject(PLATFORM_ID) private platformId: Object) { }

    setToken(token: string): void {
        if (isPlatformBrowser(this.platformId)) {
            localStorage.setItem(this.TOKEN_KEY, token);
            console.log('Set token thanh cong');
        }
    }

    getToken(): string | null {
        if (isPlatformBrowser(this.platformId)) {
            let token = localStorage.getItem(this.TOKEN_KEY);
            console.log('Lay token tu localStorage:', token);
            return token;
          }
        return null;
    }

    removeToken(): void {
        if (isPlatformBrowser(this.platformId)) {
            localStorage.removeItem(this.TOKEN_KEY);
        }
    }

    getRoleFromToken(): string | null {
        const token = this.getToken();
        if (token) {
        try {
            const payload = token.split('.')[1];
            if (!payload) {
            return null;
            }

            const decodedPayload = atob(payload);

            const parsedPayload = JSON.parse(decodedPayload);
            return parsedPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null;
        } catch (error) {
            console.error('Lỗi khi giải mã token:', error);
            return null;
        }
        }
        return null;
    }

    isAuthenticated(): boolean {
        return !!this.getToken();
    }
}