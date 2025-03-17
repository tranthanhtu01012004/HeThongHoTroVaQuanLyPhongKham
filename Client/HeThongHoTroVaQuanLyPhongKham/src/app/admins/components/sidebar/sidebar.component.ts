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
  role: string | null = null;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
    this.loadUserRole();
  }

  private loadUserRole(): void {
    this.role = this.authService.getRoleFromToken() || 'Unknown';
  }

  hasPermission(requiredRoles: string | string[]): boolean {
    const userRole = this.authService.getRoleFromToken();
    if (!userRole) return false;

    const rolesArray = Array.isArray(requiredRoles) ? requiredRoles : [requiredRoles];
    return rolesArray.includes(userRole);
  }

  logout(): void {
    this.authService.removeToken();
    this.router.navigate(['/login']);
  }
}
