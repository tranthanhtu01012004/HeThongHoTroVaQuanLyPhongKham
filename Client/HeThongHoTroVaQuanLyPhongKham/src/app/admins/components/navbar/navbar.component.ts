import { Component, ViewEncapsulation } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './navbar.component.html',
  styleUrls: [
    './navbar.component.css',
    '/public/assets/admins/css/styles.css'
  ],
  encapsulation: ViewEncapsulation.None
})
export class NavbarComponent {

}