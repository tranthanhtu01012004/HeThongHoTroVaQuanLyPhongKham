import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { IHoaDon } from '../../../interfaces/hoa-don/IHoaDon';
import { ILichHen } from '../../../interfaces/lich-hen/ILichHen';
import { IBenhNhan } from '../../../interfaces/benh-nhan/IBenhNhan';
import { IDichVuYTe } from '../../../interfaces/dich-vu-y-te/IDichVuYTe';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { HoaDonService } from '../../../services/hoa-don/hoa-don.service';
import { LichHenService } from '../../../services/lich-hen/lich-hen.service';
import { BenhNhanService } from '../../../services/benh-nhan/benh-nhan.service';
import { DichVuYTeService } from '../../../services/dich-vu-y-te/dich-vu-yte.service';
import { NotificationService } from '../../../services/handle-error/NotificationService';
import { ApiResponse } from '../../../commons/ApiResponse';
import { HttpErrorResponse } from '@angular/common/http';
import { HasPermissionDirective } from '../../../directive/has-per-mission.directive';
import { NotificationComponent } from '../../../users/components/notification/notification.component';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTableModule } from '@angular/material/table';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-hoa-don',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    NotificationComponent,
    HasPermissionDirective
  ],
  templateUrl: './hoa-don.component.html',
    styleUrls: [
      './hoa-don.component.css',
      '/public/assets/admins/css/styles.css',
      '/public/assets/admins/css/custom.css'
    ],
    encapsulation: ViewEncapsulation.None
})
export class HoaDonComponent implements OnInit {
  danhSachDaThanhToan: IHoaDon[] = [];
  danhSachChuaThanhToan: IHoaDon[] = [];
  danhSachLichHen: ILichHen[] = [];
  danhSachBenhNhan: IBenhNhan[] = [];
  danhSachDichVu: IDichVuYTe[] = [];

  soLuongDaThanhToan: number = 0;
  soLuongChuaThanhToan: number = 0;

  soLuongMoiTrangDaThanhToan: number = 3;
  soLuongMoiTrangChuaThanhToan: number = 3;

  trangHienTaiDaThanhToan: number = 0;
  trangHienTaiChuaThanhToan: number = 0;

  trangThaiThanhToanOptions = ['Đã thanh toán', 'Chưa thanh toán'];

  @ViewChild('paginatorDaThanhToan') paginatorDaThanhToan!: MatPaginator;
  @ViewChild('paginatorChuaThanhToan') paginatorChuaThanhToan!: MatPaginator;

  constructor(
    private hoaDonService: HoaDonService,
    private lichHenService: LichHenService,
    private benhNhanService: BenhNhanService,
    private dichVuYTeService: DichVuYTeService,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loadDanhSachLichHen();
    this.loadDanhSachBenhNhan();
    this.loadDanhSachDichVu();
    this.loadDanhSachHoaDon();
  }

  loadDanhSachLichHen(): void {
    this.lichHenService.getAll(1, 1000).subscribe({
      next: (res: ApiResponse<ILichHen[]>) => {
        this.danhSachLichHen = res.status ? res.data || [] : [];
      },
      error: (err: HttpErrorResponse) => this.xuLyLoi(err)
    });
  }

  loadDanhSachBenhNhan(): void {
    this.benhNhanService.getAll(1, 1000).subscribe({
      next: (res: ApiResponse<IBenhNhan[]>) => {
        this.danhSachBenhNhan = res.status ? res.data || [] : [];
      },
      error: (err: HttpErrorResponse) => this.xuLyLoi(err)
    });
  }

  loadDanhSachDichVu(): void {
    this.dichVuYTeService.getAllServices(1, 1000).subscribe({
      next: (res: ApiResponse<IDichVuYTe[]>) => {
        this.danhSachDichVu = res.status ? res.data || [] : [];
      },
      error: (err: HttpErrorResponse) => this.xuLyLoi(err)
    });
  }

  loadDanhSachHoaDon(): void {
    this.hoaDonService.getAll().subscribe({
      next: (res: ApiResponse<IHoaDon[]>) => {
        if (res.status && res.data) {
          this.phanChiaDuLieuHoaDon(res.data);
        } else {
          this.notificationService.showError('Không tải được danh sách hóa đơn.');
        }
      },
      error: (err: HttpErrorResponse) => this.xuLyLoi(err)
    });
  }

  phanChiaDuLieuHoaDon(data: IHoaDon[]): void {
    this.danhSachDaThanhToan = data.filter(hd => hd.trangThaiThanhToan === 'Đã thanh toán');
    this.danhSachChuaThanhToan = data.filter(hd => hd.trangThaiThanhToan === 'Chưa thanh toán');

    this.soLuongDaThanhToan = this.danhSachDaThanhToan.length;
    this.soLuongChuaThanhToan = this.danhSachChuaThanhToan.length;
  }

  xuLyThayDoiTrangDaThanhToan(event: PageEvent): void {
    this.trangHienTaiDaThanhToan = event.pageIndex;
    this.soLuongMoiTrangDaThanhToan = event.pageSize;
  }

  xuLyThayDoiTrangChuaThanhToan(event: PageEvent): void {
    this.trangHienTaiChuaThanhToan = event.pageIndex;
    this.soLuongMoiTrangChuaThanhToan = event.pageSize;
  }

  updateTrangThaiThanhToan(hoaDon: IHoaDon, event: Event): void {
    const select = event.target as HTMLSelectElement;
    if (select) {
      const trangThaiMoi = select.value as 'Đã thanh toán' | 'Chưa thanh toán';
      const updateData: IHoaDon = {
        ...hoaDon,
        trangThaiThanhToan: trangThaiMoi,
        ngayThanhToan: trangThaiMoi === 'Đã thanh toán' ? new Date().toISOString() : null
      };

      this.hoaDonService.updateTrangThai(hoaDon.maHoaDon, updateData).subscribe({
        next: (res: ApiResponse<IHoaDon>) => {
          if (res.status) {
            this.notificationService.showSuccess('Cập nhật trạng thái thanh toán thành công!');
            this.loadDanhSachHoaDon();
          } else {
            this.notificationService.showError(res.message || 'Cập nhật trạng thái thất bại.');
          }
        },
        error: (err: HttpErrorResponse) => {
          this.xuLyLoi(err);
          this.loadDanhSachHoaDon();
        }
      });
    }
  }

  deleteHoaDon(maHoaDon: number): void {
    if (confirm('Bạn có chắc chắn muốn xóa hóa đơn này không?')) {
      this.hoaDonService.delete(maHoaDon).subscribe({
        next: () => {
          this.notificationService.showSuccess('Xóa hóa đơn thành công!');
          this.loadDanhSachHoaDon();
        },
        error: (err: HttpErrorResponse) => this.xuLyLoi(err)
      });
    }
  }

  getTenBenhNhan(maLichHen: number): string {
    const lichHen = this.danhSachLichHen.find(lh => lh.maLichHen === maLichHen);
    if (!lichHen) return 'Không xác định';
    const benhNhan = this.danhSachBenhNhan.find(bn => bn.maBenhNhan === lichHen.maBenhNhan);
    return benhNhan ? benhNhan.ten! : 'Không xác định';
  }

  getTenDichVu(maLichHen: number): string {
    const lichHen = this.danhSachLichHen.find(lh => lh.maLichHen === maLichHen);
    if (!lichHen) return 'Không xác định';
    const dichVu = this.danhSachDichVu.find(dv => dv.maDichVuYTe === lichHen.maDichVuYTe);
    return dichVu ? dichVu.ten : 'Không xác định';
  }

  xuLyLoi(err: HttpErrorResponse): void {
    this.notificationService.handleError(err);
  }
}