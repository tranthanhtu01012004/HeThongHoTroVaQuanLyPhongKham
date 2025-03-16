import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ApiResponse } from '../../../commons/ApiResponse';
import { LichHenService } from '../../../services/lich-hen/lich-hen.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { IDichVuYTe } from '../../../interfaces/dich-vu-y-te/IDichVuYTe';
import { DichVuYTeService } from '../../../services/dich-vu-y-te/dich-vu-yte.service';
import { NotificationService } from '../../../services/handle-error/NotificationService';
import { HttpErrorResponse } from '@angular/common/http';
import { NotificationComponent } from "../notification/notification.component";
import { CommonModule } from '@angular/common';
import { LichHenCreateDTO } from '../../../interfaces/LichHenCreateDTO';
import { DateFormatterService } from '../../../services/DateFormatterService';

@Component({
  selector: 'app-lich-hen',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NotificationComponent
  ],
  templateUrl: './lich-hen.component.html',
  styleUrls: [
    './lich-hen.component.css',
    "/public/assets/users/bootstrap/owl.carousel.min.css",
    "/public/assets/users/bootstrap/tempusdominus-bootstrap-4.min.css",
    "/public/assets/users/bootstrap/bootstrap.min.css",
    "/public/assets/users/css/style.css"
  ],
  encapsulation: ViewEncapsulation.None
})
export class LichHenComponent implements OnInit {
  lichHenForm: FormGroup;
  danhSachDichVu: IDichVuYTe[] = [];
  minDateTime: string;
  maxDateTime: string;

  constructor(
    private fb: FormBuilder,
    private lichHenService: LichHenService,
    private dichVuYTeService: DichVuYTeService,
    private notificationService: NotificationService,
    private route: ActivatedRoute,
    private dateFormatter: DateFormatterService
  ) {
    this.lichHenForm = this.fb.group({
      MaDichVuYTe: ['', [Validators.required, Validators.min(1)]],
      NgayHen: ['', Validators.required],
    });

    const now = new Date();
    this.minDateTime = this.dateFormatter.formatToISOString(now).slice(0, 16);

    const maxDate = new Date(now);
    maxDate.setMonth(maxDate.getMonth() + 1); // Thêm 1 tháng
    this.maxDateTime = this.dateFormatter.formatToISOString(maxDate).slice(0, 16)
  }

  ngOnInit(): void {
    this.loadDanhSachDichVu();
    this.route.queryParams.subscribe(params => {
      const maDichVuYTe = params['maDichVuYTe'];
      if (maDichVuYTe) {
        this.lichHenForm.patchValue({ MaDichVuYTe: maDichVuYTe });
      }
    });
  }

  loadDanhSachDichVu(): void {
    this.dichVuYTeService.getAllServices(1, 1000).subscribe({
      next: (response: ApiResponse<IDichVuYTe[]>) => {
        if (response.status && response.data) {
          this.danhSachDichVu = response.data;
        } else {
          this.notificationService.showError('Không tải được danh sách dịch vụ y tế.');
        }
      },
      error: (err: HttpErrorResponse) => this.xuLyLoi(err),
    });
  }

  datLichHen(): void {
    if (this.lichHenForm.invalid) {
      this.notificationService.showError('Vui lòng chọn dịch vụ và ngày hẹn.');
      return;
    }

    const lichHen: LichHenCreateDTO = {
      MaDichVuYTe: Number(this.lichHenForm.get('MaDichVuYTe')?.value),
      NgayHen: this.dateFormatter.formatToISOString(this.lichHenForm.get('NgayHen')?.value),
    };

    this.lichHenService.createForPatient(lichHen).subscribe({
      next: (response: ApiResponse<any>) => {
        if (response.status) {
          this.notificationService.showSuccess('Đặt lịch hẹn thành công!');
          this.lichHenForm.reset();
        } else {
          this.notificationService.showError(response.message || 'Đặt lịch hẹn thất bại.');
        }
      },
      error: (err: HttpErrorResponse) => this.notificationService.handleError(err),
    });
  }

  xuLyLoi(err: HttpErrorResponse) {
    this.notificationService.handleError(err);
  }

}
