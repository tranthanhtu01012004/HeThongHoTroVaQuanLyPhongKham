import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ApiResponse } from '../../../commons/ApiResponse';
import { LichHenService } from '../../../services/lich-hen/lich-hen.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ILichHen } from '../../../interfaces/lich-hen/ILichHen';

@Component({
  selector: 'app-lich-hen',
  standalone: true,
  imports: [
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
    ReactiveFormsModule
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
  isEditMode: boolean = false;
  maLichHen: number | null = null;

  constructor(
    private fb: FormBuilder,
    private lichHenService: LichHenService,
    private route: ActivatedRoute,
    public router: Router
  ) {
    this.lichHenForm = this.fb.group({
      maBenhNhan: ['', [Validators.required, Validators.min(1)]],
      maNhanVien: ['', [Validators.required, Validators.min(1)]],
      maDichVuYTe: ['', [Validators.required, Validators.min(1)]],
      maPhongKham: ['', [Validators.required, Validators.min(1)]],
      ngayHen: ['', [Validators.required]],
      trangThai: ['', [Validators.required, Validators.pattern('^(Chờ xác nhận|Đã xác nhận|Đã hoàn thành|Hủy)$')]]
    });
  }

  ngOnInit(): void {
    this.maLichHen = +this.route.snapshot.paramMap.get('id')!;
    if (this.maLichHen) {
      this.isEditMode = true;
      this.loadLichHen();
    }
  }

  loadLichHen(): void {
    this.lichHenService.getById(this.maLichHen!).subscribe({
      next: (response: ApiResponse<ILichHen>) => {
        if (response.status && response.data) {
          this.lichHenForm.patchValue(response.data);
        } else {
          alert(response.message || 'Không tìm thấy lịch hẹn.');
        }
      },
      error: (err) => {
        console.error('Lỗi khi tải lịch hẹn:', err);
        alert('Không thể tải lịch hẹn.');
      }
    });
  }

  onSubmit(): void {
    if (this.lichHenForm.valid) {
      const lichHen: ILichHen = this.lichHenForm.value;
      if (this.isEditMode && this.maLichHen) {
        this.lichHenService.update(this.maLichHen, lichHen).subscribe({
          next: (response: ApiResponse<ILichHen>) => {
            if (response.status) {
              alert('Cập nhật lịch hẹn thành công.');
              this.router.navigate(['/lich-hen']);
            } else {
              alert(response.message || 'Cập nhật thất bại.');
            }
          },
          error: (err) => {
            console.error('Lỗi khi cập nhật:', err);
            alert('Không thể cập nhật lịch hẹn.');
          }
        });
      } else {
        this.lichHenService.add(lichHen).subscribe({
          next: (response: ApiResponse<ILichHen>) => {
            if (response.status) {
              alert('Thêm lịch hẹn thành công.');
              this.router.navigate(['/lich-hen']);
            } else {
              alert(response.message || 'Thêm thất bại.');
            }
          },
          error: (err) => {
            console.error('Lỗi khi thêm:', err);
            alert('Không thể thêm lịch hẹn.');
          }
        });
      }
    }
  }
}
