import { Component, EventEmitter, Injectable, Input, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { FormBuilder, FormGroup, NgSelectOption, ReactiveFormsModule } from '@angular/forms';
import { INhanVien } from '../../../interfaces/nhan-vien/INhanVien';
import { IPhongKham } from '../../../interfaces/phong-kham/IPhongKham';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { ILichHen } from '../../../interfaces/lich-hen/ILichHen';
import { HttpErrorResponse } from '@angular/common/http';
import { ApiResponse } from '../../../commons/ApiResponse';
import { NotificationService } from '../../../services/handle-error/NotificationService';
import { PhongKhamService } from '../../../services/phong-kham/phong-kham.service';
import { NhanVienService } from '../../../services/nhan-vien/nhan-vien.service';
import { LichHenService } from '../../../services/lich-hen/lich-hen.service';
import { IDichVuYTe } from '../../../interfaces/dich-vu-y-te/IDichVuYTe';
import { IBenhNhan } from '../../../interfaces/benh-nhan/IBenhNhan';
import { BenhNhanService } from '../../../services/benh-nhan/benh-nhan.service';
import { DichVuYTeService } from '../../../services/dich-vu-y-te/dich-vu-yte.service';
import { DateFormatterService } from '../../../services/DateFormatterService';

@Component({
  selector: 'app-appointment-filter',
  standalone: true,
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
    ReactiveFormsModule,
    MatPaginatorModule
  ],
  templateUrl: './appointment-filter.component.html',
  styleUrls: [
    './appointment-filter.component.css',
    '/public/assets/admins/css/styles.css',
    '/public/assets/admins/css/custom.css'
  ],
  encapsulation: ViewEncapsulation.None
})
export class FilterAppointmentsComponent implements OnInit {
  filterForm: FormGroup;
  danhSachLichHen: ILichHen[] = [];
  danhSachNhanVien: INhanVien[] = [];
  danhSachPhongKham: IPhongKham[] = [];
  danhSachBenhNhan: IBenhNhan[] = [];
  danhSachDichVu: IDichVuYTe[] = [];
  isLoading = false;
  currentPage = 1;
  pageSize = 10;
  totalItems = 0;
  totalPages = 0;

  constructor(
    private fb: FormBuilder,
    private lichHenService: LichHenService,
    private nhanVienService: NhanVienService,
    private phongKhamService: PhongKhamService,
    private benhNhanService: BenhNhanService,
    private dichVuYTeService: DichVuYTeService,
    private notificationService: NotificationService,
    private dateFormatter: DateFormatterService
  ) {
    this.filterForm = this.fb.group({
      ngayHen: [null],
      maNhanVien: [''],
      maPhong: ['']
    });
  }

  ngOnInit(): void {
    this.loadInitialData();
  }

  loadInitialData(): void {
    this.loadDanhSachNhanVien();
    this.loadDanhSachPhongKham();
    this.loadDanhSachBenhNhan();
    this.loadDanhSachDichVu();
    this.applyFilter();
  }

  loadDanhSachNhanVien(): void {
    this.nhanVienService.getAllServicesNotPaginator().subscribe({
      next: (res: ApiResponse<INhanVien[]>) => {
        this.danhSachNhanVien = res.status ? res.data || [] : [];
      },
      error: (err: HttpErrorResponse) => {
        this.notificationService.handleError(err);
      }
    });
  }

  loadDanhSachPhongKham(): void {
    this.phongKhamService.getAllServicesNotPaginator().subscribe({
      next: (res: ApiResponse<IPhongKham[]>) => {
        this.danhSachPhongKham = res.status ? res.data || [] : [];
      },
      error: (err: HttpErrorResponse) => {
        this.notificationService.handleError(err);
      }
    });
  }

  loadDanhSachBenhNhan(): void {
    this.benhNhanService.getAll(1, 1000).subscribe({
      next: (res: ApiResponse<IBenhNhan[]>) => {
        this.danhSachBenhNhan = res.status ? res.data || [] : [];
      },
      error: (err: HttpErrorResponse) => {
        this.notificationService.handleError(err);
      }
    });
  }

  loadDanhSachDichVu(): void {
    this.dichVuYTeService.getAllServices(1, 1000).subscribe({
      next: (res: ApiResponse<IDichVuYTe[]>) => {
        this.danhSachDichVu = res.status ? res.data || [] : [];
      },
      error: (err: HttpErrorResponse) => {
        this.notificationService.handleError(err);
      }
    });
  }

  applyFilter(): void {
    this.isLoading = true;
    const { ngayHen, maNhanVien, maPhong } = this.filterForm.value;

    let formattedNgayHen: string | undefined;
    if (ngayHen) {
      try {
        formattedNgayHen = this.dateFormatter.formatToLocalDateTime(ngayHen); // Gửi YYYY-MM-DD HH:mm:ss
      } catch (error) {
        this.notificationService.showError('Ngày hẹn không hợp lệ. Vui lòng kiểm tra lại.');
        this.isLoading = false;
        return;
      }
    }

    this.lichHenService.getAll(
      this.currentPage,
      this.pageSize,
      formattedNgayHen,
      maNhanVien ? parseInt(maNhanVien) : undefined,
      maPhong ? parseInt(maPhong) : undefined
    ).subscribe({
      next: (res: ApiResponse<ILichHen[]>) => {
        if (res.status && res.data) {
          this.danhSachLichHen = res.data;
          this.totalItems = res.totalItems || 0;
          this.totalPages = res.totalPages || 0;
          if (res.data.length === 0) {
            this.notificationService.showError('Không tìm thấy lịch hẹn phù hợp với bộ lọc.');
          } else {
            this.notificationService.showSuccess('Dữ liệu đã được lọc thành công.');
          }
        } else {
          this.notificationService.showError('Không tải được dữ liệu.');
        }
        this.isLoading = false;
      },
      error: (err: HttpErrorResponse) => {
        this.notificationService.handleError(err);
        this.isLoading = false;
      }
    });
  }

  resetFilter(): void {
    this.filterForm.reset({
      ngayHen: null,
      maNhanVien: '',
      maPhong: ''
    });
    this.currentPage = 1;
    this.applyFilter();
  }

  handlePageChange(event: PageEvent): void {
    this.currentPage = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.applyFilter();
  }

  getTenNhanVien(maNhanVien: number | null): string {
    if (!maNhanVien) return 'chưa gán vì lịch chưa được xác nhận';
    const nhanVien = this.danhSachNhanVien.find(nv => nv.maNhanVien === maNhanVien);
    return nhanVien ? nhanVien.ten : 'Không xác định';
  }

  getTenBenhNhan(maBenhNhan: number | null): string {
    if (!maBenhNhan) return 'chưa gán vì lịch chưa được xác nhận';
    const benhNhan = this.danhSachBenhNhan.find(bn => bn.maBenhNhan === maBenhNhan);
    return benhNhan && benhNhan.ten ? benhNhan.ten : 'Không xác định';
  }

  getTenDichVu(maDichVuYTe: number): string {
    const dichVu = this.danhSachDichVu.find(dv => dv.maDichVuYTe === maDichVuYTe);
    return dichVu ? dichVu.ten : 'Không xác định';
  }

  getTenPhongKham(maPhongKham: number): string {
    const phongKham = this.danhSachPhongKham.find(pk => pk.maPhongKham === maPhongKham);
    return phongKham ? phongKham.loai : 'chưa gán vì lịch chưa được xác nhận';
  }
}