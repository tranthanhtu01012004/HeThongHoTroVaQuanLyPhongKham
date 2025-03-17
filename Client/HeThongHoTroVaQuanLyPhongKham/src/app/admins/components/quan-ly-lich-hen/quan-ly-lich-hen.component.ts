import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NotificationComponent } from '../../../users/components/notification/notification.component';
import { HasPermissionDirective } from '../../../directive/has-per-mission.directive';
import { IDichVuYTe } from '../../../interfaces/dich-vu-y-te/IDichVuYTe';
import { IPhongKham } from '../../../interfaces/phong-kham/IPhongKham';
import { ILichHen } from '../../../interfaces/lich-hen/ILichHen';
import { LichHenService } from '../../../services/lich-hen/lich-hen.service';
import { PhongKhamService } from '../../../services/phong-kham/phong-kham.service';
import { NhanVienService } from '../../../services/nhan-vien/nhan-vien.service';
import { DichVuYTeService } from '../../../services/dich-vu-y-te/dich-vu-yte.service';
import { NotificationService } from '../../../services/handle-error/NotificationService';
import { DateFormatterService } from '../../../services/DateFormatterService';
import { HttpErrorResponse } from '@angular/common/http';
import { ApiResponse } from '../../../commons/ApiResponse';
import { INhanVien } from '../../../interfaces/nhan-vien/INhanVien';
import { ILichHenUpdate } from '../../../interfaces/lich-hen/ILichHenUpdate';

@Component({
  selector: 'app-quan-ly-lich-hen',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
    ReactiveFormsModule,
    NotificationComponent,
    HasPermissionDirective
  ],
  templateUrl: './quan-ly-lich-hen.component.html',
  styleUrls: [
    './quan-ly-lich-hen.component.css',
      '/public/assets/admins/css/styles.css',
      '/public/assets/admins/css/custom.css'
    ],
    encapsulation: ViewEncapsulation.None
})
export class QuanLyLichHenComponent implements OnInit {
  updateForm: FormGroup;
  danhSachPhongKham: IPhongKham[] = [];
  danhSachDichVu: IDichVuYTe[] = [];
  danhSachNhanVien: INhanVien[] = [];
  trangThaiOptions = ['Chờ xác nhận', 'Đã xác nhận', 'Hủy', 'Đã hoàn thành'];
  
   // Danh sách lịch hẹn chưa phân công
  danhSachChuaPhanCong: ILichHen[] = [];
  soLuongChuaPhanCong: number = 0;
  soLuongMoiTrangChuaPhanCong: number = 3;
  trangHienTaiChuaPhanCong: number = 0;

  // Danh sách lịch hẹn theo trạng thái
  danhSachChoXacNhan: ILichHen[] = [];
  danhSachDaXacNhan: ILichHen[] = [];
  danhSachDaHuy: ILichHen[] = [];
  danhSachDaHoanThanh: ILichHen[] = [];

  // Phân trang cho từng trạng thái
  soLuongChoXacNhan: number = 0;
  soLuongDaXacNhan: number = 0;
  soLuongDaHuy: number = 0;
  soLuongDaHoanThanh: number = 0;

  soLuongMoiTrangChoXacNhan: number = 3;
  soLuongMoiTrangDaXacNhan: number = 3;
  soLuongMoiTrangDaHuy: number = 3;
  soLuongMoiTrangDaHoanThanh: number = 3;

  trangHienTaiChoXacNhan: number = 0;
  trangHienTaiDaXacNhan: number = 0;
  trangHienTaiDaHuy: number = 0;
  trangHienTaiDaHoanThanh: number = 0;

  isEditing = false;
  selectedLichHen: ILichHen | null = null;
  minDateTime: string;

  @ViewChild('paginatorChoXacNhan') paginatorChoXacNhan!: MatPaginator;
  @ViewChild('paginatorDaXacNhan') paginatorDaXacNhan!: MatPaginator;
  @ViewChild('paginatorDaHuy') paginatorDaHuy!: MatPaginator;
  @ViewChild('paginatorDaHoanThanh') paginatorDaHoanThanh!: MatPaginator;

  constructor(
    private fb: FormBuilder,
    private lichHenService: LichHenService,
    private phongKhamService: PhongKhamService,
    private nhanVienService: NhanVienService,
    private dichVuYTeService: DichVuYTeService,
    private notificationService: NotificationService,
    private dateFormatter: DateFormatterService
  ) {
    this.updateForm = this.fb.group({
      MaLichHen: [{ value: '', disabled: true }],
      MaBenhNhan: [{ value: '', disabled: true }],
      MaDichVuYTe: [{ value: '', disabled: true }],
      MaNhanVien: [''],
      MaPhongKham: [''],
      NgayHen: ['', Validators.required],
      TrangThai: ['', Validators.required],
    });
    this.minDateTime = this.dateFormatter.formatToISOString(new Date()).slice(0, 16);
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loadDanhSachPhongKham();
    this.loadDanhSachDichVu();
    this.loadDanhSachNhanVien();
    this.loadDanhSachLichHen();
  }

  loadDanhSachPhongKham(): void {
    this.phongKhamService.getAllServicesNotPaginator().subscribe({
      next: (res: ApiResponse<IPhongKham[]>) => this.danhSachPhongKham = res.status ? res.data || [] : [],
      error: (err: HttpErrorResponse) => this.xuLyLoi(err),
    });
  }

  loadDanhSachDichVu(): void {
    this.dichVuYTeService.getAllServices(1, 1000).subscribe({
      next: (res: ApiResponse<IDichVuYTe[]>) => this.danhSachDichVu = res.status ? res.data || [] : [],
      error: (err: HttpErrorResponse) => this.xuLyLoi(err),
    });
  }

  loadDanhSachNhanVien(): void {
    this.nhanVienService.getAllServicesNotPaginator().subscribe({
      next: (res: ApiResponse<INhanVien[]>) => this.danhSachNhanVien = res.status ? res.data || [] : [],
      error: (err: HttpErrorResponse) => this.xuLyLoi(err),
    });
  }

  loadDanhSachLichHen(): void {
    this.lichHenService.getAll(1, 1000).subscribe({
      next: (res: ApiResponse<ILichHen[]>) => {
        if (res.status && res.data) {
          this.phanChiaDuLieuLichHen(res.data);
        } else {
          this.notificationService.showError('Không tải được lịch hẹn.');
        }
      },
      error: (err: HttpErrorResponse) => this.xuLyLoi(err),
    });
  }

  // Phân chia dữ liệu lịch hẹn theo trạng thái
  phanChiaDuLieuLichHen(data: ILichHen[]): void {
    this.danhSachChuaPhanCong = data.filter(lh => lh.maNhanVien === null || lh.maNhanVien === 0);
    this.danhSachChoXacNhan = data.filter(lh => lh.trangThai === 'Chờ xác nhận' && lh.maNhanVien !== null && lh.maNhanVien !== 0);
    this.danhSachDaXacNhan = data.filter(lh => lh.trangThai === 'Đã xác nhận' && lh.maNhanVien !== null && lh.maNhanVien !== 0);
    this.danhSachDaHuy = data.filter(lh => lh.trangThai === 'Hủy' && lh.maNhanVien !== null && lh.maNhanVien !== 0);
    this.danhSachDaHoanThanh = data.filter(lh => lh.trangThai === 'Đã hoàn thành' && lh.maNhanVien !== null && lh.maNhanVien !== 0);

    this.soLuongChuaPhanCong = this.danhSachChuaPhanCong.length;
    this.soLuongChoXacNhan = this.danhSachChoXacNhan.length;
    this.soLuongDaXacNhan = this.danhSachDaXacNhan.length;
    this.soLuongDaHuy = this.danhSachDaHuy.length;
    this.soLuongDaHoanThanh = this.danhSachDaHoanThanh.length;
  }

  // Xử lý thay đổi trang
  xuLyThayDoiTrangChuaPhanCong(event: PageEvent): void {
    this.trangHienTaiChuaPhanCong = event.pageIndex;
    this.soLuongMoiTrangChuaPhanCong = event.pageSize;
  }

  xuLyThayDoiTrangChoXacNhan(event: PageEvent): void {
    this.trangHienTaiChoXacNhan = event.pageIndex;
    this.soLuongMoiTrangChoXacNhan = event.pageSize;
  }

  xuLyThayDoiTrangDaXacNhan(event: PageEvent): void {
    this.trangHienTaiDaXacNhan = event.pageIndex;
    this.soLuongMoiTrangDaXacNhan = event.pageSize;
  }

  xuLyThayDoiTrangDaHuy(event: PageEvent): void {
    this.trangHienTaiDaHuy = event.pageIndex;
    this.soLuongMoiTrangDaHuy = event.pageSize;
  }

  xuLyThayDoiTrangDaHoanThanh(event: PageEvent): void {
    this.trangHienTaiDaHoanThanh = event.pageIndex;
    this.soLuongMoiTrangDaHoanThanh = event.pageSize;
  }

  editLichHen(lichHen: ILichHen): void {
    this.isEditing = true;
    this.selectedLichHen = lichHen;
    this.updateForm.patchValue({
      MaLichHen: lichHen.maLichHen,
      MaBenhNhan: lichHen.maBenhNhan,
      MaDichVuYTe: lichHen.maDichVuYTe,
      MaNhanVien: lichHen.maNhanVien || '',
      MaPhongKham: lichHen.maPhongKham || '',
      NgayHen: new Date(lichHen.ngayHen).toISOString().slice(0, 16),
      TrangThai: lichHen.trangThai,
    });
  }

  cancelEdit(): void {
    this.isEditing = false;
    this.selectedLichHen = null;
    this.updateForm.reset();
  }

  updateLichHen(): void {
    if (this.updateForm.invalid) {
      this.notificationService.showError('Vui lòng điền đầy đủ thông tin.');
      return;
    }

    const lichHenUpdate: any = {
      maNhanVien: Number(this.updateForm.value.MaNhanVien) || 0,
      maPhongKham: Number(this.updateForm.value.MaPhongKham) || 0,
    };

    this.lichHenService.update(lichHenUpdate.maLichHen, lichHenUpdate).subscribe({
      next: (res: ApiResponse<any>) => {
        if (res.status) {
          this.notificationService.showSuccess('Cập nhật thành công!');
          this.loadDanhSachLichHen();
          this.cancelEdit();
        } else {
          this.notificationService.showError(res.message || 'Cập nhật thất bại.');
        }
      },
      error: (err: HttpErrorResponse) => this.xuLyLoi(err),
    });
  }

  updateTrangThai(lichHen: ILichHen, event: Event): void {
    const select = event.target as HTMLSelectElement;
    if (select) {
      const trangThaiMoi = select.value;
      const updateData: ILichHenUpdate = { MaLichHen: lichHen.maLichHen, TrangThai: trangThaiMoi };
      this.lichHenService.updateTrangThai(lichHen.maLichHen, updateData).subscribe({
        next: (res: ApiResponse<ILichHen>) => {
          if (res.status) {
            this.notificationService.showSuccess('Cập nhật trạng thái thành công!');
            this.loadDanhSachLichHen();
          } else {
            this.notificationService.showError(res.message || 'Cập nhật trạng thái thất bại.');
          }
        },
        error: (err: HttpErrorResponse) => {
          this.xuLyLoi(err);
          this.loadDanhSachLichHen();
        }
      });
    }
  }

  getTenDichVu(maDichVuYTe: number): string {
    return this.danhSachDichVu.find(dv => dv.maDichVuYTe === maDichVuYTe)?.ten || 'Không xác định';
  }

  getTenPhongKham(maPhongKham: number): string {
    return this.danhSachPhongKham.find(pk => pk.maPhongKham === maPhongKham)?.loai || 'Chưa gán';
  }

  getTenNhanVien(maNhanVien: number | null): string {
    if (!maNhanVien) return 'Chưa gán';
    const nhanVien = this.danhSachNhanVien.find(nv => nv.maNhanVien === maNhanVien);
    return nhanVien ? nhanVien.ten : 'Không xác định';
  }

  xuLyLoi(err: HttpErrorResponse): void {
    this.notificationService.handleError(err);
  }
}