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
import { IHoSoYTeDetail } from '../../../interfaces/ho-so-y-te/IHoSoYTeDetail';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import jsPDF from 'jspdf';
import { ROBOTO_REGULAR_BASE64, ROBOTO_BOLD_BASE64 } from '../../../commons/roboto-base64';
import autoTable from 'jspdf-autotable';
import { IThuoc } from '../../../interfaces/thuoc/IThuoc';
import { ThuocService } from '../../../services/thuoc/thuoc.service';

@Component({
  selector: 'app-ho-so-y-te',
  standalone: true,
  imports: [
    CommonModule, 
    MatPaginator, 
    NotificationComponent, 
    ReactiveFormsModule, 
    HasPermissionDirective, 
    MatAutocompleteModule, 
    MatInputModule,
    MatInputModule,
    MatTabsModule,
    MatTableModule,
    MatButtonModule
  ],
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
  selectedDetail: IHoSoYTeDetail | null = null;
  showDetail: boolean = false;
  danhSachThuoc: IThuoc[] = [];
  thuocMap: Map<number, string> = new Map();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  // Cột cho bảng chi tiết
  symptomColumns: string[] = ['tenTrieuChung', 'moTa', 'thoiGianXuatHien'];
  testColumns: string[] = ['tenXetNghiem', 'ketQua', 'ngayXetNghiem'];
  prescriptionColumns: string[] = ['maThuoc', 'soLuong', 'cachDung', 'lieuLuong', 'tanSuat', 'thanhTien'];
  resultColumns: string[] = ['hieuQua', 'tacDungPhu', 'ngayDanhGia'];

  constructor(
    private hoSoYTeService: HoSoYTeService,
    private fb: FormBuilder,
    private benhNhanService: BenhNhanService,
    private notificationService: NotificationService,
    private permissionService: PermissionService,
    private thuocService: ThuocService
  ) {
    this.medicalRecordForm = this.fb.group({
      maHoSoYTe: [{ value: '', disabled: true }],
      maBenhNhan: [null, [Validators.required]],
      chuanDoan: [''],
      phuongPhapDieuTri: [''],
      lichSuBenh: ['']
    });

    this.medicalRecordForm.get('maBenhNhan')?.valueChanges.pipe(debounceTime(300)).subscribe(value => {
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
    this.loadDanhSachThuoc();
  }


  loadDanhSachThuoc(): void {
    this.thuocService.getAllServices(1, 1000).subscribe({
      next: (res: ApiResponse<IThuoc[]>) => {
        this.danhSachThuoc = res.status ? res.data || [] : [];
        this.danhSachThuoc.forEach(thuoc => {
          if (thuoc.maThuoc && thuoc.ten) {
            this.thuocMap.set(thuoc.maThuoc, thuoc.ten);
          }
        });
      },
      error: (err: HttpErrorResponse) => this.handleError(err)
    });
  }

  getTenThuoc(maThuoc: number | null): string {
    if (!maThuoc) return 'Chưa gán';
    const tenThuoc = this.thuocMap.get(maThuoc);
    return tenThuoc || 'Không xác định';
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
      this.showDetail = false;
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
      this.showDetail = false;
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
      maBenhNhan: formValue.maBenhNhan.maBenhNhan,
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

  // Xem chi tiết hồ sơ y tế
  viewMedicalRecordDetail(id: number): void {
    this.hoSoYTeService.getMedicalDetailRecord(id).subscribe({
      next: (response: ApiResponse<IHoSoYTeDetail>) => {
        if (response.status && response.data) {
          this.selectedDetail = response.data;
          this.showDetail = true;
          this.showForm = false;
        } else {
          this.notificationService.showError(response.message || 'Không tải được chi tiết hồ sơ y tế.');
        }
      },
      error: (err: HttpErrorResponse) => {
        this.handleError(err);
      }
    });
  }

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

  onBenhNhanSelected(event: MatAutocompleteSelectedEvent): void {
    const selectedBenhNhan: IBenhNhan = event.option.value;
    this.medicalRecordForm.get('maBenhNhan')?.setValue(selectedBenhNhan);
  }

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
    this.showDetail = false;
  }

  handleError(err: HttpErrorResponse): void {
    this.notificationService.handleError(err);
  }

  private initializePDF(title: string): { doc: jsPDF, pageWidth: number, margin: number, yPosition: number } {
    const doc = new jsPDF('p', 'mm', 'a4');
    const pageWidth = doc.internal.pageSize.getWidth();
    const margin = 5;
    let yPosition = margin;

    const logoWidth = 30;
    const logoHeight = 30;
    const logoX = (pageWidth - logoWidth) / 2;
    const logoUrl = '/assets/logo-clinic.png';
    doc.addImage(logoUrl, 'PNG', logoX, yPosition, logoWidth, logoHeight);
    yPosition += logoHeight + 2;

    
    const regularBase64Data = ROBOTO_REGULAR_BASE64.split(',')[1];
    doc.addFileToVFS('Roboto-Regular.ttf', regularBase64Data);
    doc.addFont('Roboto-Regular.ttf', 'Roboto', 'normal');
    
    const boldBase64Data = ROBOTO_BOLD_BASE64.split(',')[1];
    doc.addFileToVFS('Roboto-Bold.ttf', boldBase64Data);
    doc.addFont('Roboto-Bold.ttf', 'Roboto', 'bold');
    
    doc.setFont('Roboto', 'bold');
    doc.setFontSize(18);
    doc.text(title, pageWidth / 2, yPosition, { align: 'center' });
    yPosition += 10;
    
    doc.setFont('Roboto', 'normal');

    // Thêm thông tin bệnh viện
    doc.setFontSize(10);
    doc.setFont('Roboto', 'bold');
    doc.text('PHÒNG KHÁM ĐA KHOA S3T', pageWidth / 2, yPosition, { align: 'center' });
    yPosition += 5;
    doc.setFont('Roboto', 'normal');
    doc.text('Địa chỉ: 331/QL1A, Phường An Phú Đông, Quận 12, TP. HCM', pageWidth / 2, yPosition, { align: 'center' });
    yPosition += 5;
    doc.text('Điện thoại: (028) 1234 5678', pageWidth / 2, yPosition, { align: 'center' });
    yPosition += 5;
  
    yPosition += logoHeight + 2;
    
    return { doc, pageWidth, margin, yPosition };
  }
  
  // Hàm vẽ đường kẻ ngang
  private drawLine(doc: jsPDF, margin: number, yPosition: number, pageWidth: number): number {
    doc.setLineWidth(0.5);
    doc.line(margin, yPosition, pageWidth - margin, yPosition);
    return yPosition + 10;
  }
  
  printPrescription(): void {
    if (!this.selectedDetail || !this.selectedDetail.donThuoc || this.selectedDetail.donThuoc.length === 0) {
      this.notificationService.showError('Không có đơn thuốc để in.');
      return;
    }

    const { doc, pageWidth, margin, yPosition: initialY } = this.initializePDF('ĐƠN THUỐC');
    let yPosition = initialY;

    // Thông tin
    doc.setFontSize(12);
    doc.setFont('Roboto', 'normal');
    doc.text(`Mã hồ sơ y tế: ${this.selectedDetail.maHoSoYTe}`, margin, yPosition);
    yPosition += 6;
    doc.text(`Bệnh nhân: ${this.getTenBenhNhan(this.selectedDetail.maBenhNhan)}`, margin, yPosition);
    yPosition += 6;
    doc.text(`Chuẩn đoán: ${this.selectedDetail.chuanDoan}`, margin, yPosition);
    yPosition += 6;
    yPosition = this.drawLine(doc, margin, yPosition, pageWidth);

    // Tiêu đề bảng thuốc
    doc.setFontSize(14);
    doc.setFont('Roboto', 'bold');
    doc.text('Chi tiết thuốc', margin, yPosition);
    yPosition += 8;
    doc.setFont('Roboto', 'normal');

    // Vẽ bảng thuốc
    this.selectedDetail.donThuoc.forEach((donThuoc) => {
      doc.setFontSize(10);
      doc.text(`Ngày kê đơn: ${new Date(donThuoc.ngayKeDon).toLocaleString('vi-VN')}`, margin, yPosition);
      yPosition += 6;

      autoTable(doc, {
        startY: yPosition,
        head: [['Tên thuốc', 'Số lượng', 'Cách dùng', 'Liều lượng', 'Tần suất', 'Thành tiền']],
        body: donThuoc.chiTietThuocList.map((thuoc) => [
          this.getTenThuoc(thuoc.maThuoc),
          thuoc.soLuong.toString(),
          thuoc.cachDung,
          thuoc.lieuLuong,
          thuoc.tanSuat,
          thuoc.thanhTien.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' })
        ]),
        styles: { font: 'Roboto', fontSize: 10 },
        headStyles: { fillColor: [0, 102, 204], textColor: [255, 255, 255] },
        margin: { left: margin, right: margin },
        columnStyles: {
          0: { cellWidth: 50 },
          1: { cellWidth: 20 },
          2: { cellWidth: 30 },
          3: { cellWidth: 30 },
          4: { cellWidth: 30 },
          5: { cellWidth: 30 }
        }
      });
      yPosition = (doc as any).lastAutoTable.finalY + 10;
    });

    // Ghi chú và chữ ký
    doc.setFontSize(10);
    doc.text('Ghi chú: Vui lòng tuân thủ hướng dẫn của bác sĩ.', margin, yPosition);
    yPosition += 10;
    doc.setFont('Roboto', 'bold');
    doc.text('Bác sĩ kê đơn', pageWidth - margin - 30, yPosition);
    doc.line(pageWidth - margin - 30, yPosition + 2, pageWidth - margin, yPosition + 2);
    doc.setFont('Roboto', 'normal');

    doc.save(`DonThuoc_HoSo_${this.selectedDetail.maHoSoYTe}.pdf`);
  }

  printMedicalRecordDetail(): void {
    if (!this.selectedDetail) {
      this.notificationService.showError('Không có hồ sơ y tế để in.');
      return;
    }

    const { doc, pageWidth, margin, yPosition: initialY } = this.initializePDF('CHI TIẾT HỒ SƠ Y TẾ');
    let yPosition = initialY;

    // Thông tin cơ bản
    doc.setFontSize(12);
    doc.setFont('Roboto', 'normal');
    doc.text(`Mã hồ sơ y tế: ${this.selectedDetail.maHoSoYTe}`, margin, yPosition);
    yPosition += 6;
    doc.text(`Bệnh nhân: ${this.getTenBenhNhan(this.selectedDetail.maBenhNhan)}`, margin, yPosition);
    yPosition += 6;
    doc.text(`Chuẩn đoán: ${this.selectedDetail.chuanDoan}`, margin, yPosition);
    yPosition += 6;
    doc.text(`Phương pháp điều trị: ${this.selectedDetail.phuongPhapDieuTri || 'Không có'}`, margin, yPosition);
    yPosition += 6;
    doc.text(`Lịch sử bệnh: ${this.selectedDetail.lichSuBenh || 'Không có'}`, margin, yPosition);
    yPosition += 6;
    yPosition = this.drawLine(doc, margin, yPosition, pageWidth);

    // 1. Triệu chứng
    doc.setFontSize(14);
    doc.setFont('Roboto', 'bold');
    doc.text('Triệu chứng', margin, yPosition);
    yPosition += 8;
    doc.setFont('Roboto', 'normal');
    autoTable(doc, {
      startY: yPosition,
      head: [['Tên triệu chứng', 'Mô tả', 'Thời gian xuất hiện']],
      body: this.selectedDetail.trieuChung.map((tc) => [
        tc.tenTrieuChung,
        tc.moTa,
        new Date(tc.thoiGianXuatHien).toLocaleString('vi-VN')
      ]),
      styles: { font: 'Roboto', fontSize: 10 },
      headStyles: { fillColor: [0, 102, 204], textColor: [255, 255, 255] },
      margin: { left: margin, right: margin },
      columnStyles: { 0: { cellWidth: 60 }, 1: { cellWidth: 80 }, 2: { cellWidth: 50 } }
    });
    yPosition = (doc as any).lastAutoTable.finalY + 10;

    // 2. Kết quả xét nghiệm
    doc.setFontSize(14);
    doc.setFont('Roboto', 'bold');
    doc.text('Kết quả xét nghiệm', margin, yPosition);
    yPosition += 8;
    doc.setFont('Roboto', 'normal');
    autoTable(doc, {
      startY: yPosition,
      head: [['Tên xét nghiệm', 'Kết quả', 'Ngày xét nghiệm']],
      body: this.selectedDetail.ketQuaXetNghiem.map((kq) => [
        kq.tenXetNghiem,
        kq.ketQua,
        new Date(kq.ngayXetNghiem).toLocaleString('vi-VN')
      ]),
      styles: { font: 'Roboto', fontSize: 10 },
      headStyles: { fillColor: [0, 102, 204], textColor: [255, 255, 255] },
      margin: { left: margin, right: margin },
      columnStyles: { 0: { cellWidth: 60 }, 1: { cellWidth: 80 }, 2: { cellWidth: 50 } }
    });
    yPosition = (doc as any).lastAutoTable.finalY + 10;

    // 3. Đơn thuốc
    doc.setFontSize(14);
    doc.setFont('Roboto', 'bold');
    doc.text('Đơn thuốc', margin, yPosition);
    yPosition += 8;
    doc.setFont('Roboto', 'normal');
    this.selectedDetail.donThuoc.forEach((donThuoc) => {
      doc.setFontSize(10);
      doc.text(`Ngày kê đơn: ${new Date(donThuoc.ngayKeDon).toLocaleString('vi-VN')}`, margin, yPosition);
      yPosition += 6;

      autoTable(doc, {
        startY: yPosition,
        head: [['Tên thuốc', 'Số lượng', 'Cách dùng', 'Liều lượng', 'Tần suất', 'Thành tiền']],
        body: donThuoc.chiTietThuocList.map((thuoc) => [
          thuoc.maThuoc === 3 ? 'Amlodipine' : 'Unknown',
          thuoc.soLuong.toString(),
          thuoc.cachDung,
          thuoc.lieuLuong,
          thuoc.tanSuat,
          thuoc.thanhTien.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' })
        ]),
        styles: { font: 'Roboto', fontSize: 10 },
        headStyles: { fillColor: [0, 102, 204], textColor: [255, 255, 255] },
        margin: { left: margin, right: margin },
        columnStyles: {
          0: { cellWidth: 50 },
          1: { cellWidth: 20 },
          2: { cellWidth: 30 },
          3: { cellWidth: 30 },
          4: { cellWidth: 30 },
          5: { cellWidth: 30 }
        }
      });
      yPosition = (doc as any).lastAutoTable.finalY + 10;
    });

    // 4. Kết quả điều trị
    doc.setFontSize(14);
    doc.setFont('Roboto', 'bold');
    doc.text('Kết quả điều trị', margin, yPosition);
    yPosition += 8;
    doc.setFont('Roboto', 'normal');
    autoTable(doc, {
      startY: yPosition,
      head: [['Hiệu quả', 'Tác dụng phụ', 'Ngày đánh giá']],
      body: this.selectedDetail.ketQuaDieuTri.map((kq) => [
        kq.hieuQua,
        kq.tacDungPhu || 'Không có',
        new Date(kq.ngayDanhGia).toLocaleString('vi-VN')
      ]),
      styles: { font: 'Roboto', fontSize: 10 },
      headStyles: { fillColor: [0, 102, 204], textColor: [255, 255, 255] },
      margin: { left: margin, right: margin },
      columnStyles: { 0: { cellWidth: 60 }, 1: { cellWidth: 80 }, 2: { cellWidth: 50 } }
    });
    yPosition = (doc as any).lastAutoTable.finalY + 10;

    // Ghi chú và chữ ký
    doc.setFontSize(10);
    doc.text('Ghi chú: Thông tin trên được trích từ hệ thống quản lý phòng khám.', margin, yPosition);
    yPosition += 10;
    doc.setFont('Roboto', 'bold');
    doc.text('Bác sĩ phụ trách', pageWidth - margin - 30, yPosition);
    doc.line(pageWidth - margin - 30, yPosition + 2, pageWidth - margin, yPosition + 2);
    doc.setFont('Roboto', 'normal');

    doc.save(`HoSoYTe_ChiTiet_${this.selectedDetail.maHoSoYTe}.pdf`);
  }
}
