import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { NotificationComponent } from '../../../users/components/notification/notification.component';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HasPermissionDirective } from '../../../directive/has-per-mission.directive';
import { HttpErrorResponse } from '@angular/common/http';
import { IHoSoYTe } from '../../../interfaces/ho-so-y-te/IHoSoYTe';
import { ApiResponse } from '../../../commons/ApiResponse';
import { NotificationService } from '../../../services/handle-error/NotificationService';
import { PermissionService } from '../../../services/permission/permission.service';
import { HoSoYTeService } from '../../../services/ho-so-y-te/ho-so-yte.service';
import { IBenhNhan } from '../../../interfaces/benh-nhan/IBenhNhan';
import { BenhNhanService } from '../../../services/benh-nhan/benh-nhan.service';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { debounceTime } from 'rxjs';

@Component({
  selector: 'app-ho-so-y-te',
  standalone: true,
  imports: [CommonModule, MatPaginator, NotificationComponent, ReactiveFormsModule, HasPermissionDirective, MatAutocompleteModule, MatInputModule],
  templateUrl: './ho-so-y-te.component.html',
  styleUrls: [
    './ho-so-y-te.component.css',
    '/public/assets/admins/css/styles.css',
    '/public/assets/admins/css/custom.css'
  ],
  encapsulation: ViewEncapsulation.None
})
export class HoSoYTeComponent implements OnInit {
  data: IHoSoYTe[] = [];
  totalItems: number = 0;
  pageSize: number = 5;
  pageIndex: number = 0;
  showForm: boolean = false;
  editMode: boolean = false;
  selectedRecord: IHoSoYTe | null = null;
  medicalRecordForm: FormGroup;
  danhSachBenhNhan: IBenhNhan[] = [];
  filteredBenhNhan: IBenhNhan[] = [];

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private hoSoYTeService: HoSoYTeService,
    private fb: FormBuilder,
    private benhNhanService: BenhNhanService,
    private notificationService: NotificationService,
    private permissionService: PermissionService
  ) {
    this.medicalRecordForm = this.fb.group({
      maHoSoYTe: [{ value: '', disabled: true }],
      maBenhNhan: [null, [Validators.required]],
      chuanDoan: [''],
      phuongPhapDieuTri: [''],
      lichSuBenh: ['']
    });

    this.medicalRecordForm.get('maBenhNhan')?.valueChanges.pipe(
      debounceTime(300)
    ).subscribe(value => {
      if (typeof value === 'string') {
        const trimmedValue = value.trim();
        if (trimmedValue.length >= 2) {
          this.benhNhanService.getBenhNhanByName(trimmedValue).subscribe({
            next: (res: ApiResponse<IBenhNhan[]>) => {
              this.filteredBenhNhan = res.status ? (res.data || []) : [];
            },
            error: (err: HttpErrorResponse) => {
              this.handleError(err);
              this.filteredBenhNhan = [];
            }
          });
        } else {
          this.filteredBenhNhan = [];
        }
      } else {
        this.filteredBenhNhan = [];
      }
    });
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loadMedicalRecords();
    this.loadDanhSachBenhNhan();
  }

  loadMedicalRecords(): void {
    const page = this.pageIndex + 1;
    this.hoSoYTeService.getAllMedicalRecords(page, this.pageSize).subscribe({
      next: (response: ApiResponse<IHoSoYTe[]>) => {
        if (response.status && response.data) {
          this.data = response.data;
          this.totalItems = response.totalItems || response.data.length;
        } else {
          this.notificationService.showError(response.message || 'Không tải được danh sách hồ sơ y tế.');
        }
      },
      error: (err: HttpErrorResponse) => {
        this.handleError(err);
      }
    });
  }

  loadDanhSachBenhNhan(): void {
    this.benhNhanService.getAll(1, 1000).subscribe({
      next: (res: ApiResponse<IBenhNhan[]>) => {
        this.danhSachBenhNhan = res.status ? res.data || [] : [];
      },
      error: (err: HttpErrorResponse) => {
        this.handleError(err);
      }
    });
  }

  getTenBenhNhan(maBenhNhan: number): string {
    const benhNhan = this.danhSachBenhNhan.find(bn => bn.maBenhNhan === maBenhNhan);
    return benhNhan && benhNhan.ten ? benhNhan.ten : 'Không xác định';
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadMedicalRecords();
  }

  addMedicalRecord(): void {
    if (this.permissionService.hasRole('QuanLy') || this.permissionService.hasRole('BacSi') || this.permissionService.hasRole('TroLyBacSy')) {
      this.editMode = false;
      this.selectedRecord = null;
      this.medicalRecordForm.reset();
      this.showForm = true;
    }
  }

  editMedicalRecord(record: IHoSoYTe): void {
    if (this.permissionService.hasRole('QuanLy') || this.permissionService.hasRole('BacSi') || this.permissionService.hasRole('TroLyBacSy')) {
      this.editMode = true;
      this.selectedRecord = { ...record };
      this.benhNhanService.getAll(1, 1000).subscribe({
        next: (res: ApiResponse<IBenhNhan[]>) => {
          const benhNhan = res.data?.find(bn => bn.maBenhNhan === record.maBenhNhan);
          this.medicalRecordForm.patchValue({
            maHoSoYTe: record.maHoSoYTe,
            maBenhNhan: benhNhan || null,
            chuanDoan: record.chuanDoan,
            phuongPhapDieuTri: record.phuongPhapDieuTri,
            lichSuBenh: record.lichSuBenh
          });
        }
      });
      this.showForm = true;
    }
  }

  deleteMedicalRecord(id: number): void {
    if (this.permissionService.hasRole('QuanLy') || this.permissionService.hasRole('BacSi') || this.permissionService.hasRole('TroLyBacSy')) {
      if (confirm('Bạn có chắc chắn muốn xóa hồ sơ y tế này?')) {
        this.hoSoYTeService.deleteMedicalRecord(id).subscribe({
          next: () => {
            this.notificationService.showSuccess('Xóa hồ sơ y tế thành công!');
            this.loadMedicalRecords();
          },
          error: (err: HttpErrorResponse) => {
            this.handleError(err);
          }
        });
      }
    }
  }

  saveMedicalRecord(): void {
    if (this.medicalRecordForm.invalid) {
      const maBenhNhanControl = this.medicalRecordForm.get('maBenhNhan');
      if (maBenhNhanControl?.errors?.['required']) {
        this.notificationService.showError('Vui lòng chọn bệnh nhân.');
        return;
      }
      return;
    }

    const formValue = this.medicalRecordForm.value;
    const maHoSoYTe = this.medicalRecordForm.get('maHoSoYTe')?.value || 0;
    const medicalRecord: IHoSoYTe = {
      maHoSoYTe: maHoSoYTe,
      maBenhNhan: formValue.maBenhNhan.maBenhNhan, // Lấy maBenhNhan từ object
      chuanDoan: formValue.chuanDoan || null,
      phuongPhapDieuTri: formValue.phuongPhapDieuTri || null,
      lichSuBenh: formValue.lichSuBenh || null
    };

    if (this.editMode && this.selectedRecord?.maHoSoYTe) {
      this.hoSoYTeService.updateMedicalRecord(this.selectedRecord.maHoSoYTe, medicalRecord).subscribe({
        next: (response: ApiResponse<IHoSoYTe>) => {
          if (response.status) {
            this.notificationService.showSuccess('Cập nhật hồ sơ y tế thành công!');
            this.resetForm();
            this.loadMedicalRecords();
          } else {
            this.notificationService.showError(response.message || 'Cập nhật thất bại.');
          }
        },
        error: (err: HttpErrorResponse) => {
          this.handleError(err);
        }
      });
    } else {
      this.hoSoYTeService.createMedicalRecord(medicalRecord).subscribe({
        next: (response: ApiResponse<IHoSoYTe>) => {
          if (response.status) {
            this.notificationService.showSuccess('Thêm hồ sơ y tế thành công!');
            this.resetForm();
            this.loadMedicalRecords();
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

  // Tìm kiếm bệnh nhân khi nhập tên
  onSearchBenhNhan(value: string): void {
    if (value && value.length >= 2) {
      this.benhNhanService.getBenhNhanByName(value).subscribe({
        next: (res: ApiResponse<IBenhNhan[]>) => {
          this.filteredBenhNhan = res.status ? (res.data || []) : [];
        },
        error: (err: HttpErrorResponse) => {
          this.handleError(err);
          this.filteredBenhNhan = [];
        }
      });
    } else {
      this.filteredBenhNhan = [];
    }
  }

  // Xử lý khi chọn bệnh nhân từ autocomplete
  onBenhNhanSelected(event: MatAutocompleteSelectedEvent): void {
    const selectedBenhNhan: IBenhNhan = event.option.value;
    this.medicalRecordForm.get('maBenhNhan')?.setValue(selectedBenhNhan);
  }

  // Hiển thị tên bệnh nhân trong input
  displayBenhNhan(benhNhan: IBenhNhan | null): string {
    return benhNhan && benhNhan.ten ? benhNhan.ten : '';
  }

  cancelForm(): void {
    this.resetForm();
  }

  resetForm(): void {
    this.showForm = false;
    this.editMode = false;
    this.selectedRecord = null;
    this.medicalRecordForm.reset();
  }

  handleError(err: HttpErrorResponse): void {
    this.notificationService.handleError(err);
  }
}
