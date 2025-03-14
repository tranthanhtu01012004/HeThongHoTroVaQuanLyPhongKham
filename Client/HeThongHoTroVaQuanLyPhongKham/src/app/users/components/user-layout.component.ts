import { Component, ViewEncapsulation } from '@angular/core';
import { NavbarComponent } from './navbar/navbar.component';
import { FooterComponent } from './footer/footer.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-user-layout',
  standalone: true,
  imports: [
    NavbarComponent, 
    FooterComponent,
    RouterOutlet
  ],
  templateUrl: './user-layout.component.html',
  styleUrls: [
    "./user-layout.component.css",
    "/public/assets/users/bootstrap/owl.carousel.min.css",
    "/public/assets/users/bootstrap/tempusdominus-bootstrap-4.min.css",
    "/public/assets/users/bootstrap/bootstrap.min.css",
    "/public/assets/users/css/style.css"
  ],
  // tltk: https://v18.angular.dev/api/core/ViewEncapsulation (ap dung css rieng)
  encapsulation: ViewEncapsulation.None
})
export class UserLayoutComponent {

}
