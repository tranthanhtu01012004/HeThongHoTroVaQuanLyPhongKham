import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { DichVuYTeService } from '../../../services/dich-vu-y-te/dich-vu-yte.service';
import { CommonModule } from '@angular/common';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { ApiResponse } from '../../../commons/ApiResponse';
import { IDichVuYTe } from '../../../interfaces/dich-vu-y-te/IDichVuYTe';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dich-vu',
  standalone: true,
  imports: [CommonModule, MatPaginatorModule],
  templateUrl: './dich-vu.component.html',
  styleUrls: [
    './dich-vu.component.css',
    "/public/assets/users/bootstrap/owl.carousel.min.css",
    "/public/assets/users/bootstrap/tempusdominus-bootstrap-4.min.css",
    "/public/assets/users/bootstrap/bootstrap.min.css",
    "/public/assets/users/css/style.css"
  ],
  encapsulation: ViewEncapsulation.None
})
export class DichVuComponent implements OnInit {
  data: IDichVuYTe[] = [];
  totalItems: number = 0;
  pageSize: number = 3;
  pageIndex: number = 0;

  constructor(private dichVuYTeService: DichVuYTeService, private router: Router) {}

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngOnInit(): void {
      this.loadServices();
  }
  
  loadServices(): void {
    const page = this.pageIndex + 1; // MatPaginator dùng index từ 0, API dùng từ 1
    this.dichVuYTeService.getAllServices(page, this.pageSize).subscribe({
      next: (response: ApiResponse<IDichVuYTe[]>) => {
        if (response.status && response.data) {
          this.data = response.data;
          this.totalItems = response.totalItems || response.data.length;
          console.log('Services loaded:', this.data);
        }
      },
      error: (err) => {
        console.error('Error loading services:', err);
      }
    });
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadServices(); // Tải lại dữ liệu khi đổi trang
  }

  datLich(dichVu: IDichVuYTe): void {
    this.router.navigate(['/lich-hen'], { queryParams: { maDichVuYTe: dichVu.maDichVuYTe } });
  }

}
