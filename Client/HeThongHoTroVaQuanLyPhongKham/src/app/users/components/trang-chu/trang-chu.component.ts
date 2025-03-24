import { Component, ViewEncapsulation } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-trang-chu',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './trang-chu.component.html',
  styleUrls: [
    './trang-chu.component.css',
    "/public/assets/users/bootstrap/owl.carousel.min.css",
    "/public/assets/users/bootstrap/tempusdominus-bootstrap-4.min.css",
    "/public/assets/users/bootstrap/bootstrap.min.css",
    "/public/assets/users/css/style.css"
  ],
  encapsulation: ViewEncapsulation.None
})
export class TrangChuComponent {

}
