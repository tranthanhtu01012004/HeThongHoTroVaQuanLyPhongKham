import { Component, ViewEncapsulation } from '@angular/core';
import { IPhongKham } from '../../../interfaces/phong-kham/IPhongKham';

@Component({
  selector: 'app-phong-kham',
  standalone: true,
  imports: [],
  templateUrl: './phong-kham.component.html',
  styleUrls: [
    './phong-kham.component.css',
    '/public/assets/admins/css/styles.css'
  ],
  encapsulation: ViewEncapsulation.None
})
export class PhongKhamComponent {
  data: IPhongKham[] = []
  totalItems: number = 0;
  pageSize: number = 3;
  pageIndex: number = 0;

  // constructor(private phongKhamService: )
}
