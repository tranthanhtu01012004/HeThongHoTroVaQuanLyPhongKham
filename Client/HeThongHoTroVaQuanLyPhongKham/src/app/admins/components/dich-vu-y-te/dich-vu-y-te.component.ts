import { Component, ViewChild } from '@angular/core';
import { IDichVuYTe } from '../../../interfaces/dich-vu-y-te/IDichVuYTe';
import { DichVuYTeService } from '../../../services/dich-vu-y-te/dich-vu-yte.service';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ApiResponse } from '../../../commons/ApiResponse';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dich-vu-y-te',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dich-vu-y-te.component.html',
  styleUrl: './dich-vu-y-te.component.css'
})
export class DichVuYTeComponent {
  data: IDichVuYTe[] = [];
  totalItems: number = 0;
  pageSize: number = 3;
  pageIndex: number = 0;

  constructor(private dichVuYTeService: DichVuYTeService) {}

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngOnInit(): void {
      this.loadServices();
  }
  
  loadServices(): void {
    const page = this.pageIndex + 1;
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
    this.loadServices();
  }
}
