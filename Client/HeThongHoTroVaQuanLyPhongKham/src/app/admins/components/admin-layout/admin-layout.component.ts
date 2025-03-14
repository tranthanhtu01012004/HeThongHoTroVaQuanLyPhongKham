import { Component, ViewEncapsulation } from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [NavbarComponent, SidebarComponent, RouterOutlet],
  templateUrl: './admin-layout.component.html',
  styleUrls: [
    './admin-layout.component.css',
    '/public/assets/admins/css/styles.css',
  ],
  encapsulation: ViewEncapsulation.None
})
export class AdminLayoutComponent {

}
