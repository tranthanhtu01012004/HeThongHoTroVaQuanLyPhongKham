import { Component } from '@angular/core';
import { ILichHen } from '../../../interfaces/lich-hen/ILichHen';
import { LichHenService } from '../../../services/lich-hen/lich-hen.service';
import { BenhNhanService } from '../../../services/benh-nhan/benh-nhan.service';
import { AuthService } from '../../../services/Auth/AuthService';
import { ApiResponse } from '../../../commons/ApiResponse';
import { IBenhNhan } from '../../../interfaces/benh-nhan/IBenhNhan';

@Component({
  selector: 'app-quan-ly-lich-hen',
  standalone: true,
  imports: [],
  templateUrl: './quan-ly-lich-hen.component.html',
  styleUrl: './quan-ly-lich-hen.component.css'
})

export class QuanLyLichHenComponent implements OnInit {
  lichHen: ILichHen | null = null; // Lưu trữ một lịch hẹn duy nhất
  maTaiKhoan: number | null = null;
  maBenhNhan: number | null = null;

  constructor(
    private lichHenService: LichHenService,
    private benhNhanService: BenhNhanService,
    private authService: AuthService,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    if (!this.authService.isAuthenticated()) {
      this.notificationService.showError('Bạn cần đăng nhập để xem lịch hẹn.');
      return;
    }

    this.maTaiKhoan = this.authService.getMaTaiKhoanFromToken();
    if (!this.maTaiKhoan) {
      this.notificationService.showError('Không thể xác định tài khoản người dùng.');
      return;
    }

    // Lấy maBenhNhan từ maTaiKhoan và sau đó tải lịch hẹn
    this.loadMaBenhNhan().then(() => {
      if (this.maBenhNhan) {
        this.loadLichHen();
      }
    });
  }

  private async loadMaBenhNhan(): Promise<void> {
    return new Promise((resolve) => {
      this.benhNhanService.getBenhNhanByMaTaiKhoan(this.maTaiKhoan!).subscribe({
        next: (response: ApiResponse<IBenhNhan>) => {
          if (response.status && response.data) {
            this.maBenhNhan = response.data;
            console.log('MaBenhNhan loaded:', this.maBenhNhan);
          } else {
            this.notificationService.showError('Không tìm thấy thông tin bệnh nhân.');
          }
          resolve();
        },
        error: (err) => {
          this.notificationService.handleError(err);
          resolve();
        }
      });
    });
  }

  private loadLichHen(): void {
    if (this.maBenhNhan) {
      this.lichHenService.getLichHenByMaBenhNhan(this.maBenhNhan).subscribe({
        next: (response: ApiResponse<ILichHen>) => {
          if (response.status && response.data) {
            this.lichHen = response.data;
            console.log('Lich hen loaded:', this.lichHen);
          } else {
            this.lichHen = null;
            this.notificationService.showInfo('Bạn hiện chưa có lịch hẹn nào.');
          }
        },
        error: (err) => {
          this.notificationService.handleError(err);
        }
      });
    }
  }
}