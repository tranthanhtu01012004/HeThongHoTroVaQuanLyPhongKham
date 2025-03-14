import { Component, ViewChild, ViewEncapsulation } from '@angular/core';
import { IPhongKham } from '../../../interfaces/phong-kham/IPhongKham';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { PhongKhamService } from '../../../services/phong-kham/phong-kham.service';
import { NotificationService } from '../../../services/handle-error/NotificationService';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ApiResponse } from '../../../commons/ApiResponse';
import { NotificationComponent } from "../../../users/components/notification/notification.component";

@Component({
  selector: 'app-phong-kham',
  standalone: true,
  imports: [CommonModule, MatPaginator, ReactiveFormsModule, NotificationComponent],
  templateUrl: './phong-kham.component.html',
  styleUrls: [
    './phong-kham.component.css',
    '/public/assets/admins/css/styles.css'
  ],
  encapsulation: ViewEncapsulation.None
})
export class PhongKhamComponent {
  data: IPhongKham[] = []
  totalItems: number = 0;
  pageSize: number = 3;
  pageIndex: number = 0;

  showForm: boolean = false;
  editMode: boolean = false;
  selectedService: IPhongKham | null = null;
  serviceForm: FormGroup;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private phongKhamService: PhongKhamService,
    private fb: FormBuilder,
    private notificationService: NotificationService,
    private router: Router
  ) {
    this.serviceForm = this.fb.group({
      maPhongKham: [{ value: '', disabled: true }],
      loai: ['', Validators.required],
      sucChua: [0, [
        Validators.required,
        Validators.min(1),
        Validators.pattern(/^\d+$/)
      ]]
    });
  }

  ngOnInit(): void {
    this.loadServices();
  }

  loadServices(): void {
    const page = this.pageIndex + 1;
    this.phongKhamService.getAllServices(page, this.pageSize).subscribe({
      next: (response: ApiResponse<IPhongKham[]>) => {
        if (response.status && response.data) {
          this.data = response.data;
          this.totalItems = response.totalItems || response.data.length;
        } else {
          this.notificationService.showError(response.message || 'Không tải được danh sách phòng khám.');
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
    this.loadServices();
  }

  addService(): void {
    this.editMode = false;
    this.selectedService = null;
    this.serviceForm.reset({ sucChua: 0 });
    this.showForm = true;
  }

  editService(service: IPhongKham): void {
    this.editMode = true;
    this.selectedService = { ...service };
    this.serviceForm.patchValue(service);
    this.showForm = true;
  }

  saveService(): void {
    if (this.serviceForm.invalid) {
      const loaiControl = this.serviceForm.get('loai');
      const sucChuaControl = this.serviceForm.get('sucChua');

      if (loaiControl?.errors?.['required']) {
        this.notificationService.showError('Loại phòng khám là bắt buộc.');
        return;
      }

      if (sucChuaControl?.errors) {
        if (sucChuaControl.errors['required'] || sucChuaControl.value === null || sucChuaControl.value === '') {
          this.notificationService.showError('Sức chứa là bắt buộc.');
          return;
        }
        if (sucChuaControl.errors['min']) {
          this.notificationService.showError('Sức chứa phải lớn hơn 0.');
          return;
        }
        if (sucChuaControl.errors['pattern']) {
          this.notificationService.showError('Sức chứa phải là một số nguyên dương.');
          return;
        }
      }
    }

    const formValue = this.serviceForm.value;
    const maPhongKham = this.serviceForm.get('maPhongKham')?.value || 0;
    const sucChua = formValue.sucChua !== null && formValue.sucChua !== '' ? Number(formValue.sucChua) : 0;
    const service: IPhongKham = {
      maPhongKham: maPhongKham,
      loai: formValue.loai,
      sucChua: sucChua
    };

    if (this.editMode && this.selectedService?.maPhongKham) {
      this.phongKhamService.updateService(this.selectedService.maPhongKham, service).subscribe({
        next: (response: ApiResponse<IPhongKham>) => {
          if (response.status) {
            this.notificationService.showSuccess('Cập nhật phòng khám thành công!');
            this.resetForm();
            this.loadServices();
          } else {
            this.notificationService.showError(response.message || 'Cập nhật thất bại.');
          }
        },
        error: (err: HttpErrorResponse) => {
          this.handleError(err);
        }
      });
    } else {
      this.phongKhamService.createService(service).subscribe({
        next: (response: ApiResponse<IPhongKham>) => {
          if (response.status) {
            this.notificationService.showSuccess('Thêm phòng khám thành công!');
            this.resetForm();
            this.loadServices();
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

  deleteService(id: number): void {
    if (confirm('Bạn có chắc chắn muốn xóa phòng khám này?')) {
      this.phongKhamService.deleteService(id).subscribe({
        next: () => {
          this.notificationService.showSuccess('Xóa phòng khám thành công!');
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
    this.selectedService = null;
    this.serviceForm.reset({ sucChua: 0 });
  }

  handleError(err: HttpErrorResponse): void {
    this.notificationService.handleError(err);
  }
}
