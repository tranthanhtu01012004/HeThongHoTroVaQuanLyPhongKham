import { Component, ViewChild, ViewEncapsulation } from '@angular/core';
import { INhanVien } from '../../../interfaces/nhan-vien/INhanVien';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { NhanVienService } from '../../../services/nhan-vien/nhan-vien.service';
import { NotificationService } from '../../../services/handle-error/NotificationService';
import { Router } from '@angular/router';
import { ApiResponse } from '../../../commons/ApiResponse';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { NotificationComponent } from '../../../users/components/notification/notification.component';
import { HasPermissionDirective } from '../../../directive/has-per-mission.directive';
import { IVaiTro } from '../../../interfaces/vai-tro/IVaiTro';
import { VaiTroService } from '../../../services/vai-tro/vai-tro.service';

@Component({
  selector: 'app-nhan-vien',
  standalone: true,
  imports: [CommonModule, MatPaginator, NotificationComponent, ReactiveFormsModule, HasPermissionDirective],
  templateUrl: './nhan-vien.component.html',
    styleUrls: [
      './nhan-vien.component.css',
      '/public/assets/admins/css/styles.css',
      '/public/assets/admins/css/custom.css'
    ],
    encapsulation: ViewEncapsulation.None
})
export class NhanVienComponent {
  data: INhanVien[] = []
  vaiTroList: IVaiTro[] = [];
  totalItems: number = 0;
  pageSize: number = 3;
  pageIndex: number = 0;

  showForm: boolean = false;
  editMode: boolean = false;
  selectedNhanVien: INhanVien | null = null;
  nhanVienForm: FormGroup;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private nhanVienService: NhanVienService,
    private fb: FormBuilder,
    private notificationService: NotificationService,
    private vaiTroService: VaiTroService
  ) {
    this.nhanVienForm = this.fb.group({
      maNhanVien: [{ value: 0, disabled: true }],
      ten: ['', Validators.required],
      soDienThoai: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
      Email: ['', [Validators.email]],
      caLamViec: ['', Validators.required],
      chuyenMon: ['', Validators.required],
      tenDangNhap: ['', Validators.required],
      matKhau: ['', Validators.required],
      maVaiTro: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadNhanViens();
    this.loadVaiTros();
  }

  loadVaiTros(): void {
    this.vaiTroService.getAllServices().subscribe({
      next: (response: any) => {
        if (response.status && response.data) {
          console.log(response.data);
          // Lọc bỏ vai trò "QuanLy"
          this.vaiTroList = response.data.filter((vaiTro: IVaiTro) => vaiTro.ten !== 'QuanLy');
          console.log('vaiTroList after filtering:', this.vaiTroList); // Kiểm tra dữ liệu sau khi lọc
        } else {
          this.notificationService.showError('Không tải được danh sách vai trò.');
        }
      },
      error: (err: HttpErrorResponse) => {
        this.handleError(err);
      }
    });
  }
  
  loadNhanViens(): void {
    const page = this.pageIndex + 1;
    this.nhanVienService.getAllServices(page, this.pageSize).subscribe({
      next: (response: ApiResponse<INhanVien[]>) => {
        if (response.status && response.data) {
          this.data = response.data;
          this.totalItems = response.totalItems || response.data.length;
        } else {
          this.notificationService.showError(response.message || 'Không tải được danh sách nhân viên.');
        }
      },
      error: (err: HttpErrorResponse) => {
        this.handleError(err);
      }
    });
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadNhanViens();
  }

  addNhanVien(): void {
    this.editMode = false;
    this.selectedNhanVien = null;
    this.nhanVienForm.reset({ maNhanVien: 0 });
    this.nhanVienForm.reset({ maNhanVien: 0, caLamViec: '' });
    this.showForm = true;
  }

  editNhanVien(nhanVien: INhanVien): void {
    this.editMode = true;
    this.selectedNhanVien = { ...nhanVien };
    this.nhanVienForm.patchValue(nhanVien);
    this.showForm = true;
  }

  saveNhanVien(): void {
    const formValue = this.nhanVienForm.value;
    const nhanVien: INhanVien = {
      maNhanVien: this.editMode ? this.selectedNhanVien?.maNhanVien || 0 : 0,
      ten: formValue.ten,
      soDienThoai: formValue.soDienThoai,
      caLamViec: formValue.caLamViec || undefined,
      chuyenMon: formValue.chuyenMon,
      tenDangNhap: formValue.tenDangNhap,
      matKhau: formValue.matKhau,
      maVaiTro: Number(formValue.maVaiTro)
    };

    if (this.editMode && this.selectedNhanVien?.maNhanVien) {
      this.nhanVienService.updateService(this.selectedNhanVien.maNhanVien, nhanVien).subscribe({
        next: (response: ApiResponse<INhanVien>) => {
          if (response.status) {
            this.notificationService.showSuccess('Cập nhật nhân viên thành công!');
            this.resetForm();
            this.loadNhanViens();
          } else {
            this.notificationService.showError(response.message || 'Cập nhật thất bại.');
          }
        },
        error: (err: HttpErrorResponse) => {
          this.handleError(err);
        }
      });
    } else {
      this.nhanVienService.createService(nhanVien).subscribe({
        next: (response: ApiResponse<INhanVien>) => {
          if (response.status) {
            this.notificationService.showSuccess('Thêm nhân viên thành công!');
            this.resetForm();
            this.loadNhanViens();
          } else {
            this.notificationService.showError(response.message || 'Thêm thất bại.');
          }
        },
        error: (err: HttpErrorResponse) => {
          this.handleError(err);
        }
      });
    }
  }

  deleteNhanVien(id: number): void {
    if (confirm('Bạn có chắc chắn muốn xóa nhân viên này?')) {
      this.nhanVienService.deleteService(id).subscribe({
        next: () => {
          this.notificationService.showSuccess('Xóa nhân viên thành công!');
          setTimeout(() => {
            window.location.reload();
          }, 1000);
        },
        error: (err: HttpErrorResponse) => {
          this.handleError(err);
        }
      });
    }
  }

  cancelForm(): void {
    this.resetForm();
  }

  resetForm(): void {
    this.showForm = false;
    this.editMode = false;
    this.selectedNhanVien = null;
    this.nhanVienForm.reset({ maNhanVien: 0 });
    this.nhanVienForm.reset({ maNhanVien: 0, caLamViec: '' })
  }

  handleError(err: HttpErrorResponse): void {
    this.notificationService.handleError(err);
  }
}
