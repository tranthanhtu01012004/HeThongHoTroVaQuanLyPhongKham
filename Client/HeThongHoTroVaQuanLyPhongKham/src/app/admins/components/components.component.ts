import { Component } from '@angular/core';
import { AppComponent } from "../../app.component";
import { NavbarComponent } from './navbar/navbar.component';
import { FooterComponent } from './footer/footer.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-components',
  standalone: true,
  imports: [],
  templateUrl: './components.component.html',
  styleUrl: './components.component.css'
})
export class ComponentsComponent {

}
