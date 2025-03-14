import { Component, ViewChild } from '@angular/core';
import { IDichVuYTe } from '../../../interfaces/dich-vu-y-te/IDichVuYTe';
import { DichVuYTeService } from '../../../services/dich-vu-y-te/dich-vu-yte.service';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ApiResponse } from '../../../commons/ApiResponse';
import { CommonModule } from '@angular/common';
import { NotificationService } from '../../../services/handle-error/NotificationService';
import { HttpErrorResponse } from '@angular/common/http';
import { ApiErrorResponse } from '../../../commons/ApiErrorResponse';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NotificationComponent } from "../../../users/components/notification/notification.component";
import { Router } from '@angular/router';

@Component({
  selector: 'app-dich-vu-y-te',
  standalone: true,
  imports: [CommonModule, MatPaginator, NotificationComponent, ReactiveFormsModule],
  templateUrl: './dich-vu-y-te.component.html',
  styleUrl: './dich-vu-y-te.component.css'
})
export class DichVuYTeComponent {
  data: IDichVuYTe[] = [];
  totalItems: number = 0;
  pageSize: number = 3;
  pageIndex: number = 0;
  showForm: boolean = false;
  editMode: boolean = false;
  selectedService: IDichVuYTe | null = null;
  serviceForm: FormGroup;

  private readonly MAX_DECIMAL_VALUE = 79228162514264337593543950335;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private dichVuYTeService: DichVuYTeService,
    private fb: FormBuilder,
    private notificationService: NotificationService,
    private router: Router
  ) {
    this.serviceForm = this.fb.group({
      maDichVuYTe: [{ value: '', disabled: true }],
      ten: ['', Validators.required],
      chiPhi: [0, [
        Validators.required,
        Validators.min(0),
        Validators.max(this.MAX_DECIMAL_VALUE),
        Validators.pattern(/^\d*\.?\d+$/)
      ]]
    });
  }

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
        } else {
          this.notificationService.showError(response.message || 'Không tải được danh sách dịch vụ.');
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
    this.serviceForm.reset({ chiPhi: 0 });
    this.showForm = true;
  }

  editService(service: IDichVuYTe): void {
    this.editMode = true;
    this.selectedService = { ...service };
    this.serviceForm.patchValue(service);
    this.showForm = true;
  }

  saveService(): void {
    if (this.serviceForm.invalid) {
      const chiPhiControl = this.serviceForm.get('chiPhi');
  
      if (chiPhiControl?.errors) {
        if (chiPhiControl.errors['required'] || chiPhiControl.value === null || chiPhiControl.value === '') {
          this.notificationService.showError('Chi phí là bắt buộc.');
          return;
        }
        if (chiPhiControl.errors['min']) {
          this.notificationService.showError('Chi phí không được nhỏ hơn 0.');
          return;
        }
        if (chiPhiControl.errors['max']) {
          this.notificationService.showError('Chi phí vượt quá giá trị tối đa cho phép.');
          return;
        }
        if (chiPhiControl.errors['pattern']) {
          this.notificationService.showError('Chi phí phải là một số hợp lệ.');
          return;
        }
      }
    }

    const formValue = this.serviceForm.value;
    const maDichVuYTe = this.serviceForm.get('maDichVuYTe')?.value || 0;
    const service: IDichVuYTe = {
      maDichVuYTe: maDichVuYTe,
      ten: formValue.ten,
      chiPhi: formValue.chiPhi
    };

    if (this.editMode && this.selectedService?.maDichVuYTe) {
      this.dichVuYTeService.updateService(this.selectedService.maDichVuYTe, service).subscribe({
        next: (response: ApiResponse<IDichVuYTe>) => {
          if (response.status) {
            this.notificationService.showSuccess('Cập nhật dịch vụ thành công!');
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
      this.dichVuYTeService.createService(service).subscribe({
        next: (response: ApiResponse<IDichVuYTe>) => {
          if (response.status) {
            this.notificationService.showSuccess('Thêm dịch vụ thành công!');
            this.resetForm();
            this.loadServices();
          } else {
            this.notificationService.showError(response.message || 'Thêm thất bại.');
          }
        },
        error: (err: HttpErrorResponse) => {
          this.handleError(err); // Lỗi từ server sẽ được xử lý ở đây
        }
      });
    }
  }

  deleteService(id: number): void {
    if (confirm('Bạn có chắc chắn muốn xóa dịch vụ này?')) {
      this.dichVuYTeService.deleteService(id).subscribe({
        next: () => {
            this.notificationService.showSuccess('Xóa dịch vụ thành công!');
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
    this.serviceForm.reset({ chiPhi: 0 });
  }

  handleError(err: HttpErrorResponse): void {
    this.notificationService.handleError(err);
  }
}
