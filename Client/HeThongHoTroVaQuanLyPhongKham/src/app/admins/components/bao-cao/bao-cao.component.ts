import { Component, signal, ViewChild, ViewEncapsulation } from "@angular/core";
import { BaoCaoService } from "../../../services/bao-cao/bao-cao.service";
import { CommonModule } from "@angular/common";
import { ChartConfiguration, ChartData, ChartOptions, ChartType } from 'chart.js';
import { BaseChartDirective, NgChartsModule } from 'ng2-charts';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { NotificationComponent } from "../../../users/components/notification/notification.component";
import { MatPaginator, PageEvent } from "@angular/material/paginator";
import { HasPermissionDirective } from "../../../directive/has-per-mission.directive";
import { DoanhThuReportDTO } from "../../../interfaces/bao-cao/DoanhThuReportDTO";
import { LichHenReportDTO } from "../../../interfaces/bao-cao/LichHenReportDTO";
import { DonThuocReportDTO } from "../../../interfaces/bao-cao/DonThuocReportDTO";
import { DichVuYTeReportDTO } from "../../../interfaces/bao-cao/DichVuYTeReportDTO";
import { NotificationService } from "../../../services/handle-error/NotificationService";
import { PermissionService } from "../../../services/permission/permission.service";
import { HttpErrorResponse } from "@angular/common/http";

@Component({
  selector: 'app-bao-cao',
  standalone: true,
  imports: [
    CommonModule,
    NgChartsModule,
    ReactiveFormsModule,
    NotificationComponent,
    MatPaginator,
    HasPermissionDirective
  ],
  templateUrl: './bao-cao.component.html',
  styleUrls: [
    './bao-cao.component.css',
    '/public/assets/admins/css/styles.css',
    '/public/assets/admins/css/custom.css'
  ],
  encapsulation: ViewEncapsulation.None
})
export class BaoCaoComponent {
  filterForm: FormGroup;
  doanhThuReport: DoanhThuReportDTO | null = null;
  lichHenReport: LichHenReportDTO | null = null;
  donThuocReport: DonThuocReportDTO | null = null;
  dichVuYTeReport: DichVuYTeReportDTO | null = null;

  danhSachHoaDon: any[] = [];
  paginatedHoaDon: any[] = [];
  totalItems: number = 0;
  pageSize: number = 3;
  pageIndex: number = 0;

  doanhThuChartType: ChartType = 'bar';
  doanhThuChartData: ChartData<'bar'> = { labels: [], datasets: [] };
  doanhThuChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: 'top',
        labels: {
          font: {
            size: 14,
            family: 'Arial'
          },
          color: '#333'
        }
      },
      tooltip: {
        backgroundColor: 'rgba(0, 0, 0, 0.8)',
        titleFont: { size: 14 },
        bodyFont: { size: 12 },
        callbacks: {
          label: (context) => {
            const value = context.raw as number;
            return `${context.dataset.label}: ${value.toLocaleString('vi-VN')} VNĐ`;
          }
        }
      }
    },
    scales: {
      x: {
        ticks: {
          font: {
            size: 12,
            family: 'Arial'
          },
          color: '#666'
        },
        grid: {
          display: false
        }
      },
      y: {
        beginAtZero: true,
        ticks: {
          font: {
            size: 12,
            family: 'Arial'
          },
          color: '#666',
          callback: (value) => value.toLocaleString('vi-VN')
        },
        grid: {
          color: 'rgba(0, 0, 0, 0.1)'
        }
      }
    }
  };
  @ViewChild('doanhThuChart') doanhThuChart!: BaseChartDirective;

  lichHenChartType: ChartType = 'pie';
  lichHenChartData: ChartData<'pie'> = { labels: [], datasets: [] };
  lichHenChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: 'right',
        labels: {
          font: {
            size: 14,
            family: 'Arial'
          },
          color: '#333'
        }
      },
      tooltip: {
        backgroundColor: 'rgba(0, 0, 0, 0.8)',
        titleFont: { size: 14 },
        bodyFont: { size: 12 }
      }
    }
  };
  @ViewChild('lichHenChart') lichHenChart!: BaseChartDirective;

  donThuocChartType: ChartType = 'bar';
  donThuocChartData: ChartData<'bar'> = { labels: [], datasets: [] };
  donThuocChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: 'top',
        labels: {
          font: {
            size: 14,
            family: 'Arial'
          },
          color: '#333'
        }
      },
      tooltip: {
        backgroundColor: 'rgba(0, 0, 0, 0.8)',
        titleFont: { size: 14 },
        bodyFont: { size: 12 }
      }
    },
    scales: {
      x: {
        ticks: {
          font: {
            size: 12,
            family: 'Arial'
          },
          color: '#666'
        },
        grid: {
          display: false
        }
      },
      y: {
        beginAtZero: true,
        ticks: {
          font: {
            size: 12,
            family: 'Arial'
          },
          color: '#666'
        },
        grid: {
          color: 'rgba(0, 0, 0, 0.1)'
        }
      }
    }
  };
  @ViewChild('donThuocChart') donThuocChart!: BaseChartDirective;

  dichVuYTeChartType: ChartType = 'bar';
  dichVuYTeChartData: ChartData<'bar'> = { labels: [], datasets: [] };
  dichVuYTeChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: 'top',
        labels: {
          font: {
            size: 14,
            family: 'Arial'
          },
          color: '#333'
        }
      },
      tooltip: {
        backgroundColor: 'rgba(0, 0, 0, 0.8)',
        titleFont: { size: 14 },
        bodyFont: { size: 12 }
      }
    },
    scales: {
      x: {
        ticks: {
          font: {
            size: 12,
            family: 'Arial'
          },
          color: '#666'
        },
        grid: {
          display: false
        }
      },
      y: {
        beginAtZero: true,
        ticks: {
          font: {
            size: 12,
            family: 'Arial'
          },
          color: '#666'
        },
        grid: {
          color: 'rgba(0, 0, 0, 0.1)'
        }
      }
    }
  };
  @ViewChild('dichVuYTeChart') dichVuYTeChart!: BaseChartDirective;

  isLoading: boolean = false;
  errorMessage: string | null = null;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private baoCaoService: BaoCaoService,
    private fb: FormBuilder,
    private notificationService: NotificationService,
    private permissionService: PermissionService
  ) {
    this.filterForm = this.fb.group({
      tuNgay: ['2025-12-07', Validators.required],
      denNgay: ['2025-12-14', Validators.required],
      trangThaiThanhToan: ['Đã thanh toán']
    });
  }

  ngOnInit(): void {
    this.loadReports();
  }

  loadReports(): void {
    if (this.filterForm.invalid) {
      this.notificationService.showError('Vui lòng nhập đầy đủ thông tin lọc.');
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;
    const { tuNgay, denNgay, trangThaiThanhToan } = this.filterForm.value;
    const tuNgayDate = new Date(tuNgay);
    const denNgayDate = new Date(denNgay);
    const trangThai = trangThaiThanhToan === 'Tất cả' ? undefined : trangThaiThanhToan;

    if (tuNgayDate > denNgayDate) {
      this.notificationService.showError('Ngày bắt đầu không thể lớn hơn ngày kết thúc.');
      this.isLoading = false;
      return;
    }

    this.baoCaoService.thongKeDoanhThu(tuNgayDate, denNgayDate, trangThai).subscribe({
      next: (data) => {
        this.doanhThuReport = data;
        this.danhSachHoaDon = data.danhSachHoaDon;
        this.totalItems = data.danhSachHoaDon.length;
        this.updatePaginatedHoaDon();
        this.doanhThuChartData = {
          labels: data.danhSachHoaDon.map(hd => `Hóa Đơn ${hd.maHoaDon}`),
          datasets: [
            {
              label: 'Tổng Tiền (VNĐ)',
              data: data.danhSachHoaDon.map(hd => hd.tongTien),
              backgroundColor: 'rgba(75, 192, 192, 0.6)',
              borderColor: 'rgba(75, 192, 192, 1)',
              borderWidth: 1,
              hoverBackgroundColor: 'rgba(75, 192, 192, 0.8)',
              hoverBorderColor: 'rgba(75, 192, 192, 1)'
            }
          ]
        };
        if (this.doanhThuChart) {
          this.doanhThuChart.update('show');
        }
      },
      error: (err: HttpErrorResponse) => {
        this.handleError(err);
      }
    });

    this.baoCaoService.thongKeLichHen(tuNgayDate, denNgayDate).subscribe({
      next: (data) => {
        this.lichHenReport = data;
        this.lichHenChartData = {
          labels: Object.keys(data.soLuongTheoTrangThai),
          datasets: [
            {
              label: 'Số Lượng',
              data: Object.values(data.soLuongTheoTrangThai),
              backgroundColor: [
                'rgba(255, 99, 132, 0.6)',
                'rgba(54, 162, 235, 0.6)',
                'rgba(255, 206, 86, 0.6)',
                'rgba(75, 192, 192, 0.6)'
              ],
              borderColor: [
                'rgba(255, 99, 132, 1)',
                'rgba(54, 162, 235, 1)',
                'rgba(255, 206, 86, 1)',
                'rgba(75, 192, 192, 1)'
              ],
              borderWidth: 1,
              hoverOffset: 20
            }
          ]
        };
        if (this.lichHenChart) {
          this.lichHenChart.update('show');
        }
      },
      error: (err: HttpErrorResponse) => {
        this.handleError(err);
      }
    });

    this.baoCaoService.thongKeDonThuoc(tuNgayDate, denNgayDate).subscribe({
      next: (data) => {
        this.donThuocReport = data;
        this.donThuocChartData = {
          labels: Object.keys(data.soLuongTheoBenhNhan).map(id => `Bệnh Nhân ${id}`),
          datasets: [
            {
              label: 'Số Đơn Thuốc',
              data: Object.values(data.soLuongTheoBenhNhan),
              backgroundColor: 'rgba(153, 102, 255, 0.6)',
              borderColor: 'rgba(153, 102, 255, 1)',
              borderWidth: 1,
              hoverBackgroundColor: 'rgba(153, 102, 255, 0.8)',
              hoverBorderColor: 'rgba(153, 102, 255, 1)'
            }
          ]
        };
        if (this.donThuocChart) {
          this.donThuocChart.update('show');
        }
      },
      error: (err: HttpErrorResponse) => {
        this.handleError(err);
      }
    });

    this.baoCaoService.thongKeDichVuYTe(tuNgayDate, denNgayDate).subscribe({
      next: (data) => {
        this.dichVuYTeReport = data;
        this.dichVuYTeChartData = {
          labels: Object.keys(data.soLuongTheoDichVu),
          datasets: [
            {
              label: 'Số Bệnh Nhân',
              data: Object.values(data.soLuongTheoDichVu),
              backgroundColor: 'rgba(255, 159, 64, 0.6)',
              borderColor: 'rgba(255, 159, 64, 1)',
              borderWidth: 1,
              hoverBackgroundColor: 'rgba(255, 159, 64, 0.8)',
              hoverBorderColor: 'rgba(255, 159, 64, 1)'
            }
          ]
        };
        if (this.dichVuYTeChart) {
          this.dichVuYTeChart.update('show');
        }
      },
      error: (err: HttpErrorResponse) => {
        this.handleError(err);
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }

  updatePaginatedHoaDon(): void {
    const startIndex = this.pageIndex * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedHoaDon = this.danhSachHoaDon.slice(startIndex, endIndex);
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.updatePaginatedHoaDon();
  }

  handleError(err: HttpErrorResponse): void {
    this.notificationService.handleError(err);
    this.errorMessage = 'Không thể tải dữ liệu báo cáo. Vui lòng thử lại sau.';
    this.isLoading = false;
  }
}