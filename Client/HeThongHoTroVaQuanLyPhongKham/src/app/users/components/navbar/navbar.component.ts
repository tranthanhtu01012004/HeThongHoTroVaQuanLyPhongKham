import { CommonModule } from '@angular/common';
import { Component, ViewEncapsulation } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/Auth/AuthService';
import { LoginStore } from '../../../store/LoginStore';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './navbar.component.html',
  styleUrls: [
    "./navbar.component.css",
    "/public/assets/users/bootstrap/owl.carousel.min.css",
    "/public/assets/users/bootstrap/tempusdominus-bootstrap-4.min.css",
    "/public/assets/users/bootstrap/bootstrap.min.css",
    "/public/assets/users/css/style.css"
  ],
  encapsulation: ViewEncapsulation.None
})
export class NavbarComponent {
  constructor(
    public authService: AuthService,
    private router: Router,
    private loginStore: LoginStore
  ) { }

  logout(): void {
    this.authService.removeToken();
    this.loginStore.setAuthenticated(false);
    this.loginStore.setRole('');
    console.log('Login successful:', this.authService.getToken());
    this.router.navigate(['/dang-nhap']);
  }
}
