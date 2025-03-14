import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private readonly TOKEN_KEY = 'auth_token';
    private readonly ROLE_KEY = 'auth_role';

    setToken(token: string): void {
        localStorage.setItem(this.TOKEN_KEY,token);
    }

    getToken(): string | null {
        return localStorage.getItem(this.TOKEN_KEY);
    }

    removeToken(): void {
        localStorage.removeItem(this.TOKEN_KEY);
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