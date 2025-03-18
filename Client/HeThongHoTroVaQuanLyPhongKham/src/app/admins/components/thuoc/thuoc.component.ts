import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { IThuoc } from '../../../interfaces/thuoc/IThuoc';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { ApiResponse } from '../../../commons/ApiResponse';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ThuocService } from '../../../services/thuoc/thuoc.service';
import { NotificationService } from '../../../services/handle-error/NotificationService';
import { PermissionService } from '../../../services/permission/permission.service';
import { CommonModule } from '@angular/common';
import { NotificationComponent } from '../../../users/components/notification/notification.component';
import { HasPermissionDirective } from '../../../directive/has-per-mission.directive';

@Component({
  selector: 'app-thuoc',
  standalone: true,
  imports: [
    CommonModule,
    MatPaginator,
    NotificationComponent,
    ReactiveFormsModule,
    HasPermissionDirective
  ],
  templateUrl: './thuoc.component.html',
  styleUrls: [
    './thuoc.component.css',
    '/public/assets/admins/css/styles.css',
    '/public/assets/admins/css/custom.css'
  ],
  encapsulation: ViewEncapsulation.None
})
export class ThuocComponent implements OnInit {
  data: IThuoc[] = [];
  totalItems: number = 0;
  pageSize: number = 3;
  pageIndex: number = 0;
  showForm: boolean = false;
  editMode: boolean = false;
  selectedThuoc: IThuoc | null = null;
  thuocForm: FormGroup;

  donViOptions: string[] = [
    'viên',       // Thuốc viên (tablet, capsule)
    'tuýp',       // Kem bôi, gel
    'chai',       // Dung dịch truyền, thuốc nước
    'gói',        // Thuốc bột, gói cốm
    'ống',        // Thuốc tiêm, ống uống
    'lọ',         // Thuốc dạng lọ (ví dụ: insulin)
    'hộp',        // Đóng gói dạng hộp
    'ml',         // Dạng dung dịch đo bằng ml
    'mg',         // Dạng bột hoặc viên tính bằng mg
    'vỉ'          // Thuốc đóng vỉ (blister pack)
  ];

  private readonly MAX_DECIMAL_VALUE = 79228162514264337593543950335;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private thuocService: ThuocService,
    private fb: FormBuilder,
    private notificationService: NotificationService,
    private permissionService: PermissionService
  ) {
    this.thuocForm = this.fb.group({
      maThuoc: [{ value: '', disabled: true }],
      ten: ['', Validators.required],
      moTa: [''],
      donVi: ['', Validators.required],
      chongChiDinh: [''],
      tuongTacThuoc: [''],
      donGia: [0, [
        Validators.required,
        Validators.min(0),
        Validators.max(this.MAX_DECIMAL_VALUE),
        Validators.pattern(/^\d+$/)
      ]]
    });
  }

  ngOnInit(): void {
    this.loadThuoc();
  }

  loadThuoc(): void {
    const page = this.pageIndex + 1;
    this.thuocService.getAllServices(page, this.pageSize).subscribe({
      next: (response: ApiResponse<IThuoc[]>) => {
        if (response.status && response.data) {
          this.data = response.data;
          this.totalItems = response.totalItems || response.data.length;
        } else {
          this.notificationService.showError(response.message || 'Không tải được danh sách thuốc.');
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
    this.loadThuoc();
  }

  addThuoc(): void {
    if (this.permissionService.hasAnyRole(['QuanLy', 'DuocSi'])) {
      this.editMode = false;
      this.selectedThuoc = null;
      this.thuocForm.reset({ donGia: 0, donVi: '' });
      this.showForm = true;
    }
  }

  editThuoc(thuoc: IThuoc): void {
    if (this.permissionService.hasAnyRole(['QuanLy', 'DuocSi'])) {
      this.editMode = true;
      this.selectedThuoc = { ...thuoc };
      this.thuocForm.patchValue(thuoc);
      this.thuocForm.patchValue({
        maThuoc: thuoc.maThuoc,
        ten: thuoc.ten,
        moTa: thuoc.moTa,
        donVi: thuoc.donVi,
        chongChiDinh: thuoc.chongChiDinh,
        tuongTacThuoc: thuoc.tuongTacThuoc,
        donGia: thuoc.donGia
      });
      this.showForm = true;
    }
  }

  deleteThuoc(id: number): void {
    if (this.permissionService.hasAnyRole(['QuanLy', 'DuocSi'])) {
      if (confirm('Bạn có chắc chắn muốn xóa thuốc này?')) {
        this.thuocService.deleteService(id).subscribe({
          next: () => {
            this.notificationService.showSuccess('Xóa thuốc thành công!');
            this.loadThuoc();
          },
          error: (err: HttpErrorResponse) => {
            this.handleError(err);
          }
        });
      }
    }
  }

  saveThuoc(): void {
    if (this.thuocForm.invalid) {
      const donGiaControl = this.thuocForm.get('donGia');
      if (donGiaControl?.errors?.['required'] || donGiaControl?.value === null || donGiaControl?.value === '') {
        this.notificationService.showError('Vui lòng nhập đơn giá.');
        return;
      }
      if (donGiaControl?.errors?.['min']) {
        this.notificationService.showError('Đơn giá không được nhỏ hơn 0.');
        return;
      }
      if (donGiaControl?.errors?.['max']) {
        this.notificationService.showError('Đơn giá vượt quá giá trị tối đa cho phép.');
        return;
      }
      if (donGiaControl?.errors?.['pattern']) {
        this.notificationService.showError('Đơn giá phải là một số hợp lệ và không được là số thập phân.');
        return;
      }
      this.notificationService.showError('Vui lòng điền đầy đủ thông tin bắt buộc.');
      return;
    }

    const formValue = this.thuocForm.value;
    const maThuoc = this.thuocForm.get('maThuoc')?.value || 0;
    const thuoc: IThuoc = {
      maThuoc: maThuoc,
      ten: formValue.ten,
      moTa: formValue.moTa,
      donVi: formValue.donVi,
      chongChiDinh: formValue.chongChiDinh,
      tuongTacThuoc: formValue.tuongTacThuoc,
      donGia: formValue.donGia
    };

    if (this.editMode && this.selectedThuoc?.maThuoc) {
      this.thuocService.updateService(this.selectedThuoc.maThuoc, thuoc).subscribe({
        next: (response: ApiResponse<IThuoc>) => {
          if (response.status) {
            this.notificationService.showSuccess('Cập nhật thuốc thành công!');
            this.resetForm();
            this.loadThuoc();
          } else {
            this.notificationService.showError(response.message || 'Cập nhật thất bại.');
          }
        },
        error: (err: HttpErrorResponse) => {
          this.handleError(err);
        }
      });
    } else {
      this.thuocService.createService(thuoc).subscribe({
        next: (response: ApiResponse<IThuoc>) => {
          if (response.status) {
            this.notificationService.showSuccess('Thêm thuốc thành công!');
            this.resetForm();
            this.loadThuoc();
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

  cancelForm(): void {
    this.resetForm();
  }

  resetForm(): void {
    this.showForm = false;
    this.editMode = false;
    this.selectedThuoc = null;
    this.thuocForm.reset({ donGia: 0, donVi: '' });
  }

  handleError(err: HttpErrorResponse): void {
    this.notificationService.handleError(err);
  }
}
