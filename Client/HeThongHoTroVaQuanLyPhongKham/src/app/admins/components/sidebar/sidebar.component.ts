import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/Auth/AuthService';
import { LoginStore } from '../../../store/LoginStore';
import { HasPermissionDirective } from '../../../directive/has-per-mission.directive';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './sidebar.component.html',
  styleUrls: [
    './sidebar.component.css',
    '/public/assets/admins/css/styles.css'
  ],
  encapsulation: ViewEncapsulation.None
})
export class SidebarComponent implements OnInit{
  role: string = '';
  constructor(
      public authService: AuthService,
      private router: Router,
      private loginStore: LoginStore
    ) { }
  
  ngOnInit(): void {
    this.loadRole();
  }

  loadRole(): void {
    const role = this.authService.getRoleFromToken();
    this.role = role ? role : '';
  }
  
  logout(): void {
    this.authService.removeToken();
    this.loginStore.setAuthenticated(false);
    this.loginStore.setRole('');
    console.log('Logout successful:', this.authService.getToken());
    this.router.navigate(['/dang-nhap']);
  }
}
