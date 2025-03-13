import { Component } from '@angular/core';
import { NavbarComponent } from './navbar/navbar.component';
import { FooterComponent } from './footer/footer.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [NavbarComponent, FooterComponent, RouterOutlet],
  templateUrl: './components.component.html',
  styleUrls: [
    './components.component.css',
    '../../../../public/assets/users/bootstrap/owl.carousel.min.css',
    '../../../../public/assets/users/bootstrap/tempusdominus-bootstrap-4.min.css',
    '../../../../public/assets/users/bootstrap/bootstrap.min.css',
    '../../../../public/assets/users/css/style.css'
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class ComponentsComponent {

}
