import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { NotificationComponent } from '../../../users/components/notification/notification.component';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
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
import { ITrieuChung } from '../../../interfaces/trieu-chung/ITrieuChung';
import { IKetQuaXetNghiem } from '../../../interfaces/ket-qua-xet-nghiem/IKetQuaXetNghiem';
import { IDonThuoc } from '../../../interfaces/don-thuoc/IDonThuoc';
import { IKetQuaDieuTri } from '../../../interfaces/ket-qua-deu-tri/IKetQuaDieuTri';
import { TrieuChungService } from '../../../services/trieu-chung/trieu-chung.service';
import { KetQuaXetNghiemService } from '../../../services/ket-qua-xet-nghiem/ket-qua-xet-nghiem.service';
import { DonThuocService } from '../../../services/don-thuoc/don-thuoc.service';
import { KetQuaDieuTriService } from '../../../services/ket-qua-dieu-tri/ket-qua-dieu-tri.service';

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
    MatTabsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule
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
  danhSachHoSo: IHoSoYTe[] = [];
  tongSoBanGhi: number = 0;
  soBanGhiMoiTrang: number = 5;
  trangHienTai: number = 0;
  hienForm: boolean = false;
  dangSua: boolean = false;
  hoSoDangChon: IHoSoYTe | null = null;
  formHoSoYTe: FormGroup;
  danhSachBenhNhan: IBenhNhan[] = [];
  danhSachBenhNhanLoc: IBenhNhan[] = [];
  chiTietHoSo: IHoSoYTeDetail | null = null;
  hienChiTiet: boolean = false;
  danhSachThuoc: IThuoc[] = [];
  danhSachThuocLoc: IThuoc[][] = [];
  bangTenThuoc: Map<number, string> = new Map();

  // Form triệu chứng
  hienFormTrieuChung: boolean = false;
  dangSuaTrieuChung: boolean = false;
  trieuChungDangChon: ITrieuChung | null = null;
  formTrieuChung: FormGroup;

  // Form kết quả xét nghiệm
  hienFormKetQuaXetNghiem: boolean = false;
  dangSuaKetQuaXetNghiem: boolean = false;
  ketQuaXetNghiemDangChon: IKetQuaXetNghiem | null = null;
  formKetQuaXetNghiem: FormGroup;

  // Form đơn thuốc
  hienFormDonThuoc: boolean = false;
  dangSuaDonThuoc: boolean = false;
  donThuocDangChon: IDonThuoc | null = null;
  formDonThuoc: FormGroup;

  // Form kết quả điều trị
  hienFormKetQuaDieuTri: boolean = false;
  dangSuaKetQuaDieuTri: boolean = false;
  ketQuaDieuTriDangChon: IKetQuaDieuTri | null = null;
  formKetQuaDieuTri: FormGroup;

  @ViewChild(MatPaginator) phanTrang!: MatPaginator;

  cotTrieuChung: string[] = ['tenTrieuChung', 'moTa', 'thoiGianXuatHien'];
  cotKetQuaXetNghiem: string[] = ['tenXetNghiem', 'ketQua', 'ngayXetNghiem'];
  cotDonThuoc: string[] = ['maThuoc', 'soLuong', 'cachDung', 'lieuLuong', 'tanSuat', 'thanhTien'];
  cotKetQuaDieuTri: string[] = ['hieuQua', 'tacDungPhu', 'ngayDanhGia'];

  constructor(
    private hoSoYTeService: HoSoYTeService,
    private fb: FormBuilder,
    private benhNhanService: BenhNhanService,
    private notificationService: NotificationService,
    private thuocService: ThuocService,
    private trieuChungService: TrieuChungService,
    private ketQuaXetNghiemService: KetQuaXetNghiemService,
    private donThuocService: DonThuocService,
    private ketQuaDieuTriService: KetQuaDieuTriService,
  ) {
    this.formHoSoYTe = this.fb.group({
      maHoSoYTe: [{ value: '', disabled: true }],
      maBenhNhan: [null, [Validators.required]],
      chuanDoan: [''],
      phuongPhapDieuTri: [''],
      lichSuBenh: ['']
    });

    this.formTrieuChung = this.fb.group({
      maTrieuChung: [0],
      maHoSoYTe: [0],
      tenTrieuChung: ['', Validators.required],
      moTa: [''],
      thoiGianXuatHien: ['', Validators.required]
    });

    this.formKetQuaXetNghiem = this.fb.group({
      maKetQuaXetNghiem: [0],
      maHoSoYTe: [0],
      tenXetNghiem: ['', Validators.required],
      ketQua: ['', Validators.required],
      ngayXetNghiem: ['', Validators.required]
    });

    this.formDonThuoc = this.fb.group({
      maDonThuoc: [0],
      maHoSoYTe: [0],
      maHoaDon: [0, Validators.required],
      ngayKeDon: ['', Validators.required],
      chiTietThuocList: this.fb.array([])
    });

    this.formKetQuaDieuTri = this.fb.group({
      maKetQuaDieuTri: [0],
      maHoSoYTe: [0],
      hieuQua: ['', Validators.required],
      tacDungPhu: [''],
      ngayDanhGia: ['', Validators.required]
    });

    this.formHoSoYTe.get('maBenhNhan')?.valueChanges.pipe(debounceTime(300)).subscribe(value => {
      if (typeof value === 'string') {
        const giaTri = value.trim();
        if (giaTri.length >= 2) {
          this.benhNhanService.getBenhNhanByName(giaTri).subscribe({
            next: (res: ApiResponse<IBenhNhan[]>) => {
              this.danhSachBenhNhanLoc = res.status ? (res.data || []) : [];
            },
            error: (err: HttpErrorResponse) => {
              this.xuLyLoi(err);
            }
          });
        } else {
          this.danhSachBenhNhanLoc = [];
        }
      } else {
        this.danhSachBenhNhanLoc = [];
      }
    });
  }

  ngOnInit(): void {
    this.taiDuLieu();
  }

  taiDuLieu(): void {
    this.taiDanhSachHoSo();
    this.taiDanhSachBenhNhan();
    this.taiDanhSachThuoc();
  }

  taiDanhSachThuoc(): void {
    this.thuocService.getAllServices(1, 1000).subscribe({
      next: (res: ApiResponse<IThuoc[]>) => {
        this.danhSachThuoc = res.status ? res.data || [] : [];
        this.danhSachThuoc.forEach(thuoc => {
          if (thuoc.maThuoc && thuoc.ten) {
            this.bangTenThuoc.set(thuoc.maThuoc, thuoc.ten);
          }
        });
      },
      error: (err: HttpErrorResponse) => this.xuLyLoi(err)
    });
  }

  getTenThuoc(maThuoc: number | null): string {
    if (!maThuoc) return 'Chưa gán';
    const tenThuoc = this.bangTenThuoc.get(maThuoc);
    return tenThuoc || 'Không xác định';
  }

  taiDanhSachHoSo(): void {
    const trang = this.trangHienTai + 1;
    this.hoSoYTeService.getAllMedicalRecords(trang, this.soBanGhiMoiTrang).subscribe({
      next: (response: ApiResponse<IHoSoYTe[]>) => {
        if (response.status && response.data) {
          this.danhSachHoSo = response.data;
          this.tongSoBanGhi = response.totalItems || response.data.length;
        } else {
          this.notificationService.showError(response.message || 'Không tải được danh sách hồ sơ y tế.');
        }
      },
      error: (err: HttpErrorResponse) => {
        this.xuLyLoi(err);
      }
    });
  }

  taiDanhSachBenhNhan(): void {
    this.benhNhanService.getAll(1, 1000).subscribe({
      next: (res: ApiResponse<IBenhNhan[]>) => {
        this.danhSachBenhNhan = res.status ? res.data || [] : [];
      },
      error: (err: HttpErrorResponse) => {
        this.xuLyLoi(err);
      }
    });
  }

  getTenBenhNhan(maBenhNhan: number): string {
    const benhNhan = this.danhSachBenhNhan.find(bn => bn.maBenhNhan === maBenhNhan);
    return benhNhan && benhNhan.ten ? benhNhan.ten : 'Không xác định';
  }

  onPageChange(suKien: PageEvent): void {
    this.trangHienTai = suKien.pageIndex;
    this.soBanGhiMoiTrang = suKien.pageSize;
    this.taiDanhSachHoSo();
  }

  xemChiTietHoSo(id: number): void {
    this.hoSoYTeService.getMedicalDetailRecord(id).subscribe({
      next: (response: ApiResponse<IHoSoYTeDetail>) => {
        if (response.status && response.data) {
          this.chiTietHoSo = response.data;
          this.hienChiTiet = true;
          this.hienForm = false;
        } else {
          this.notificationService.showError(response.message || 'Không tải được chi tiết hồ sơ y tế.');
        }
      },
      error: (err: HttpErrorResponse) => {
        this.xuLyLoi(err);
      }
    });
  }

  moFormThemHoSo(): void {
    this.dangSua = false;
    this.hoSoDangChon = null;
    this.formHoSoYTe.reset();
    this.hienForm = true;
    this.hienChiTiet = false;
  }

  suaHoSo(hoSo: IHoSoYTe): void {
    this.dangSua = true;
    this.hoSoDangChon = hoSo;
    this.formHoSoYTe.patchValue({
      maHoSoYTe: hoSo.maHoSoYTe,
      maBenhNhan: hoSo.maBenhNhan,
      chuanDoan: hoSo.chuanDoan,
      phuongPhapDieuTri: hoSo.phuongPhapDieuTri,
      lichSuBenh: hoSo.lichSuBenh
    });
    this.hienForm = true;
    this.hienChiTiet = false;
  }

  luuHoSo(): void {
    if (this.formHoSoYTe.invalid) {
      this.notificationService.showError('Vui lòng điền đầy đủ thông tin hồ sơ y tế.');
      return;
    }

    const hoSo: IHoSoYTe = this.formHoSoYTe.getRawValue();
    if (this.dangSua && this.hoSoDangChon?.maHoSoYTe) {
      this.hoSoYTeService.updateMedicalRecord(this.hoSoDangChon.maHoSoYTe, hoSo).subscribe({
        next: (response: ApiResponse<IHoSoYTe>) => {
          if (response.status) {
            this.notificationService.showSuccess('Cập nhật hồ sơ y tế thành công!');
            this.taiDanhSachHoSo();
            this.huyForm();
          } else {
            this.notificationService.showError(response.message || 'Cập nhật thất bại.');
          }
        },
        error: (err: HttpErrorResponse) => this.xuLyLoi(err)
      });
    } else {
      this.hoSoYTeService.createMedicalRecord(hoSo).subscribe({
        next: (response: ApiResponse<IHoSoYTe>) => {
          if (response.status) {
            this.notificationService.showSuccess('Thêm hồ sơ y tế thành công!');
            this.taiDanhSachHoSo();
            this.huyForm();
          } else {
            this.notificationService.showError(response.message || 'Thêm thất bại.');
          }
        },
        error: (err: HttpErrorResponse) => this.xuLyLoi(err)
      });
    }
  }

  xoaHoSo(hoSo: IHoSoYTe): void {
    if (confirm('Bạn có chắc chắn muốn xóa hồ sơ y tế này?')) {
      this.hoSoYTeService.deleteMedicalRecord(hoSo.maHoSoYTe!).subscribe({
        next: () => {
          this.notificationService.showSuccess('Xóa hồ sơ y tế thành công!');
          this.taiDanhSachHoSo();
        },
        error: (err: HttpErrorResponse) => this.xuLyLoi(err)
      });
    }
  }

  huyForm(): void {
    this.hienForm = false;
    this.dangSua = false;
    this.hoSoDangChon = null;
    this.formHoSoYTe.reset();
  }

  resetForm(): void {
    this.hienChiTiet = false;
    this.hienForm = false;
    this.chiTietHoSo = null;
    this.formHoSoYTe.reset();
    this.dangSua = false;
    this.hoSoDangChon = null;
  }

  xuLyLoi(err: HttpErrorResponse): void {
    this.notificationService.handleError(err);
  }

  // Triệu chứng
  moFormThemTrieuChung(): void {
    this.hienFormTrieuChung = true;
    this.dangSuaTrieuChung = false;
    this.formTrieuChung = this.fb.group({
      maTrieuChung: [{ value: '', disabled: true }],
      maHoSoYte: [this.chiTietHoSo?.maHoSoYTe, [Validators.required, Validators.min(1)]],
      tenTrieuChung: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(200)]],
      moTa: ['', [Validators.minLength(1), Validators.maxLength(500)]],
      thoiGianXuatHien: ['', Validators.required]
    });
  }

  suaTrieuChung(trieuChung: ITrieuChung): void {
    this.hienFormTrieuChung = true;
    this.dangSuaTrieuChung = true;
    this.formTrieuChung = this.fb.group({
      maTrieuChung: [{ value: trieuChung.maTrieuChung, disabled: true }],
      maHoSoYte: [this.chiTietHoSo?.maHoSoYTe, [Validators.required, Validators.min(1)]],
      tenTrieuChung: [trieuChung.tenTrieuChung, [Validators.required, Validators.minLength(1), Validators.maxLength(200)]],
      moTa: [trieuChung.moTa, [Validators.minLength(1), Validators.maxLength(500)]],
      thoiGianXuatHien: [trieuChung.thoiGianXuatHien, Validators.required]
    });
  }

  luuTrieuChung(): void {
    const formValue = this.formTrieuChung.value;
    const maTrieuChung = this.formTrieuChung.get('maTrieuChung')?.value || 0;
    const trieuChung: ITrieuChung = {
      maTrieuChung: maTrieuChung,
      maHoSoYTe: formValue.maHoSoYte,
      tenTrieuChung: formValue.tenTrieuChung,
      moTa: formValue.moTa,
      thoiGianXuatHien: formValue.thoiGianXuatHien
    };
    if (this.dangSuaTrieuChung) {
      this.trieuChungService.updateService(trieuChung.maTrieuChung!, trieuChung).subscribe({
        next: (response: ApiResponse<ITrieuChung>) => {
          if (response.status) {
            this.notificationService.showSuccess('Cập nhật triệu chứng thành công!');
            this.xemChiTietHoSo(this.chiTietHoSo!.maHoSoYTe);
            this.huyFormTrieuChung();
          } else {
            this.notificationService.showError(response.message || 'Cập nhật thất bại.');
          }
        },
        error: (err: HttpErrorResponse) => this.xuLyLoi(err)
      });
    } else {
      this.trieuChungService.createService(trieuChung).subscribe({
        next: (response: ApiResponse<ITrieuChung>) => {
          if (response.status) {
            this.notificationService.showSuccess('Thêm triệu chứng thành công!');
            this.xemChiTietHoSo(this.chiTietHoSo!.maHoSoYTe);
            this.huyFormTrieuChung();
          } else {
            this.notificationService.showError(response.message || 'Thêm thất bại.');
          }
        },
        error: (err: HttpErrorResponse) => this.xuLyLoi(err)
      });
    }
  }

  xoaTrieuChung(trieuChung: ITrieuChung): void {
    if (confirm('Bạn có chắc chắn muốn xóa triệu chứng này?')) {
      this.trieuChungService.deleteService(trieuChung.maTrieuChung!).subscribe({
        next: () => {
          this.notificationService.showSuccess('Xóa triệu chứng thành công!');
          this.xemChiTietHoSo(this.chiTietHoSo!.maHoSoYTe);
        },
        error: (err: HttpErrorResponse) => this.xuLyLoi(err)
      });
    }
  }

  huyFormTrieuChung(): void {
    this.hienFormTrieuChung = false;
    this.dangSuaTrieuChung = false;
    this.trieuChungDangChon = null;
    this.formTrieuChung.reset();
  }

  // Kết quả xét nghiệm
  moFormThemKetQuaXetNghiem(): void {
    this.dangSuaKetQuaXetNghiem = false;
    this.ketQuaXetNghiemDangChon = null;
    this.formKetQuaXetNghiem.reset();
    this.formKetQuaXetNghiem.patchValue({ maHoSoYTe: this.chiTietHoSo?.maHoSoYTe });
    this.hienFormKetQuaXetNghiem = true;
  }

  suaKetQuaXetNghiem(ketQua: IKetQuaXetNghiem): void {
    this.dangSuaKetQuaXetNghiem = true;
    this.ketQuaXetNghiemDangChon = ketQua;
    this.formKetQuaXetNghiem.patchValue({
      maKetQuaXetNghiem: ketQua.maKetQua,
      maHoSoYTe: this.chiTietHoSo?.maHoSoYTe,
      tenXetNghiem: ketQua.tenXetNghiem,
      ketQua: ketQua.ketQua,
      ngayXetNghiem: new Date(ketQua.ngayXetNghiem).toISOString().slice(0, 16)
    });
    this.hienFormKetQuaXetNghiem = true;
  }

  luuKetQuaXetNghiem(): void {
    if (this.formKetQuaXetNghiem.invalid) {
      this.notificationService.showError('Vui lòng điền đầy đủ thông tin kết quả xét nghiệm.');
      return;
    }

    const ketQua: IKetQuaXetNghiem = this.formKetQuaXetNghiem.value;
    if (this.dangSuaKetQuaXetNghiem && this.ketQuaXetNghiemDangChon?.maKetQua) { 
      this.ketQuaXetNghiemService.updateService(this.ketQuaXetNghiemDangChon.maKetQua, ketQua).subscribe({
        next: (response: ApiResponse<IKetQuaXetNghiem>) => {
          if (response.status) {
            this.notificationService.showSuccess('Cập nhật kết quả xét nghiệm thành công!');
            this.xemChiTietHoSo(this.chiTietHoSo!.maHoSoYTe);
            this.huyFormKetQuaXetNghiem();
          } else {
            this.notificationService.showError(response.message || 'Cập nhật thất bại.');
          }
        },
        error: (err: HttpErrorResponse) => this.xuLyLoi(err)
      });
    } else {
      this.ketQuaXetNghiemService.createService(ketQua).subscribe({
        next: (response: ApiResponse<IKetQuaXetNghiem>) => {
          if (response.status) {
            this.notificationService.showSuccess('Thêm kết quả xét nghiệm thành công!');
            this.xemChiTietHoSo(this.chiTietHoSo!.maHoSoYTe);
            this.huyFormKetQuaXetNghiem();
          } else {
            this.notificationService.showError(response.message || 'Thêm thất bại.');
          }
        },
        error: (err: HttpErrorResponse) => this.xuLyLoi(err)
      });
    }
  }

  xoaKetQuaXetNghiem(ketQua: IKetQuaXetNghiem): void {
    if (confirm('Bạn có chắc chắn muốn xóa kết quả xét nghiệm này?')) {
      this.ketQuaXetNghiemService.deleteService(ketQua.maKetQua!).subscribe({
        next: () => {
          this.notificationService.showSuccess('Xóa kết quả xét nghiệm thành công!');
          this.xemChiTietHoSo(this.chiTietHoSo!.maHoSoYTe);
        },
        error: (err: HttpErrorResponse) => this.xuLyLoi(err)
      });
    }
  }

  huyFormKetQuaXetNghiem(): void {
    this.hienFormKetQuaXetNghiem = false;
    this.dangSuaKetQuaXetNghiem = false;
    this.ketQuaXetNghiemDangChon = null;
    this.formKetQuaXetNghiem.reset();
  }

  // Đơn thuốc
  get danhSachChiTietThuoc(): FormArray {
    return this.formDonThuoc.get('chiTietThuocList') as FormArray;
  }

  themChiTietThuoc(): void {
    const chiTietThuoc = this.fb.group({
      maThuoc: this.fb.control<string | number | null>(null, Validators.required),
      soLuong: [1, [Validators.required, Validators.min(1)]],
      cachDung: ['', Validators.required],
      lieuLuong: ['', Validators.required],
      tanSuat: ['', Validators.required],
      thanhTien: [0]
    });
    this.danhSachChiTietThuoc.push(chiTietThuoc);

    const index = this.danhSachChiTietThuoc.length - 1;
    this.danhSachThuocLoc[index] = [...this.danhSachThuoc];

    // Lắng nghe sự kiện thay đổi của maThuoc để lọc thuốc
    chiTietThuoc.get('maThuoc')?.valueChanges.pipe(debounceTime(300)).subscribe(value => {
      if (typeof value === 'string') {
        const giaTri = value.trim().toLowerCase();
        if (giaTri.length >= 1) {
          this.danhSachThuocLoc[index] = this.danhSachThuoc.filter(thuoc =>
            thuoc.ten?.toLowerCase().includes(giaTri) || thuoc.maThuoc?.toString().includes(giaTri)
          );
        } else {
          this.danhSachThuocLoc[index] = [...this.danhSachThuoc]; // Hiển thị lại toàn bộ danh sách nếu không có từ khóa
        }
      } else {
        this.danhSachThuocLoc[index] = [...this.danhSachThuoc]; // Reset danh sách nếu người dùng chọn một giá trị
      }
    });
  }

  xoaChiTietThuoc(chiSo: number): void {
    this.danhSachChiTietThuoc.removeAt(chiSo);
    this.danhSachThuocLoc.splice(chiSo, 1);
  }

  chonThuoc(suKien: MatAutocompleteSelectedEvent, chiSo: number): void {
    const maThuoc = suKien.option.value;
    this.danhSachChiTietThuoc.at(chiSo).patchValue({ maThuoc });
    this.tinhThanhTien(chiSo);
  }

  tinhThanhTien(chiSo: number): void {
    const chiTietThuoc = this.danhSachChiTietThuoc.at(chiSo);
    const maThuoc = chiTietThuoc.get('maThuoc')?.value;
    const soLuong = chiTietThuoc.get('soLuong')?.value;
    const thuoc = this.danhSachThuoc.find(t => t.maThuoc === maThuoc);
    if (thuoc && thuoc.donGia) {
      const thanhTien = soLuong * thuoc.donGia;
      chiTietThuoc.patchValue({ thanhTien });
    }
  }

  moFormThemDonThuoc(): void {
    this.dangSuaDonThuoc = false;
    this.donThuocDangChon = null;
    this.formDonThuoc.reset();
    this.danhSachChiTietThuoc.clear();
    this.themChiTietThuoc();
    this.formDonThuoc.patchValue({ maHoSoYTe: this.chiTietHoSo?.maHoSoYTe });
    this.hienFormDonThuoc = true;
  }

  suaDonThuoc(donThuoc: IDonThuoc): void {
    this.dangSuaDonThuoc = true;
    this.donThuocDangChon = donThuoc;
    this.formDonThuoc.patchValue({
      maDonThuoc: donThuoc.maDonThuoc,
      maHoSoYTe: this.chiTietHoSo?.maHoSoYTe,
      ngayKeDon: new Date(donThuoc.ngayKeDon).toISOString().slice(0, 16)
    });
    this.danhSachChiTietThuoc.clear();
    donThuoc.chiTietThuocList.forEach(thuoc => {
      const chiTietThuoc = this.fb.group({
        maThuoc: [thuoc.maThuoc, Validators.required],
        soLuong: [thuoc.soLuong, [Validators.required, Validators.min(1)]],
        cachDung: [thuoc.cachDung, Validators.required],
        lieuLuong: [thuoc.lieuLuong, Validators.required],
        tanSuat: [thuoc.tanSuat, Validators.required],
        thanhTien: [thuoc.thanhTien]
      });
      this.danhSachChiTietThuoc.push(chiTietThuoc);
    });
    this.hienFormDonThuoc = true;
  }

  luuDonThuoc(): void {
    if (this.formDonThuoc.invalid) {
      this.notificationService.showError('Vui lòng điền đầy đủ thông tin đơn thuốc.');
      return;
    }

    const formValue = this.formDonThuoc.value;
    const maDonThuoc = this.formTrieuChung.get('maDonThuoc')?.value || 0;
    const donThuoc: IDonThuoc = {
      maDonThuoc: maDonThuoc,
      maHoSoYTe: formValue.maHoSoYTe || 0,
      maHoaDon: formValue.maHoaDon,
      ngayKeDon: formValue.ngayKeDon,
      chiTietThuocList: formValue.chiTietThuocList
    };
    if (this.dangSuaDonThuoc && this.donThuocDangChon?.maDonThuoc) {
      this.donThuocService.updateService(this.donThuocDangChon.maDonThuoc, donThuoc).subscribe({
        next: (response: ApiResponse<IDonThuoc>) => {
          if (response.status) {
            this.notificationService.showSuccess('Cập nhật đơn thuốc thành công!');
            this.xemChiTietHoSo(this.chiTietHoSo!.maHoSoYTe);
            this.huyFormDonThuoc();
          } else {
            this.notificationService.showError(response.message || 'Cập nhật thất bại.');
          }
        },
        error: (err: HttpErrorResponse) => this.xuLyLoi(err)
      });
    } else {
      this.donThuocService.createService(donThuoc).subscribe({
        next: (response: ApiResponse<IDonThuoc>) => {
          if (response.status) {
            this.notificationService.showSuccess('Thêm đơn thuốc thành công!');
            this.xemChiTietHoSo(this.chiTietHoSo!.maHoSoYTe);
            this.huyFormDonThuoc();
          } else {
            this.notificationService.showError(response.message || 'Thêm thất bại.');
          }
        },
        error: (err: HttpErrorResponse) => this.xuLyLoi(err)
      });
    }
  }

  xoaDonThuoc(donThuoc: IDonThuoc): void {
    if (donThuoc.maHoaDon) {
      this.notificationService.showError('Không thể xóa đơn thuốc đã có hóa đơn.');
      return;
    }
    if (confirm('Bạn có chắc chắn muốn xóa đơn thuốc này?')) {
      this.donThuocService.deleteService(donThuoc.maDonThuoc!).subscribe({
        next: () => {
          this.notificationService.showSuccess('Xóa đơn thuốc thành công!');
          this.xemChiTietHoSo(this.chiTietHoSo!.maHoSoYTe);
        },
        error: (err: HttpErrorResponse) => this.xuLyLoi(err)
      });
    }
  }

  huyFormDonThuoc(): void {
    this.hienFormDonThuoc = false;
    this.dangSuaDonThuoc = false;
    this.donThuocDangChon = null;
    this.formDonThuoc.reset();
    this.danhSachChiTietThuoc.clear();
  }

  // Kết quả điều trị
  moFormThemKetQuaDieuTri(): void {
    this.dangSuaKetQuaDieuTri = false;
    this.ketQuaDieuTriDangChon = null;
    this.formKetQuaDieuTri.reset();
    this.formKetQuaDieuTri.patchValue({ maHoSoYTe: this.chiTietHoSo?.maHoSoYTe });
    this.hienFormKetQuaDieuTri = true;
  }

  suaKetQuaDieuTri(ketQua: IKetQuaDieuTri): void {
    this.dangSuaKetQuaDieuTri = true;
    this.ketQuaDieuTriDangChon = ketQua;
    this.formKetQuaDieuTri.patchValue({
      maKetQuaDieuTri: ketQua.maKetQuaDieuTri,
      maHoSoYTe: this.chiTietHoSo?.maHoSoYTe,
      hieuQua: ketQua.hieuQua,
      tacDungPhu: ketQua.tacDungPhu,
      ngayDanhGia: new Date(ketQua.ngayDanhGia).toISOString().slice(0, 16)
    });
    this.hienFormKetQuaDieuTri = true;
  }

  luuKetQuaDieuTri(): void {
    if (this.formKetQuaDieuTri.invalid) {
      this.notificationService.showError('Vui lòng điền đầy đủ thông tin kết quả điều trị.');
      return;
    }

    const ketQua: IKetQuaDieuTri = this.formKetQuaDieuTri.value;
    if (this.dangSuaKetQuaDieuTri && this.ketQuaDieuTriDangChon?.maKetQuaDieuTri) {
      this.ketQuaDieuTriService.updateService(this.ketQuaDieuTriDangChon.maKetQuaDieuTri, ketQua).subscribe({
        next: (response: ApiResponse<IKetQuaDieuTri>) => {
          if (response.status) {
            this.notificationService.showSuccess('Cập nhật kết quả điều trị thành công!');
            this.xemChiTietHoSo(this.chiTietHoSo!.maHoSoYTe);
            this.huyFormKetQuaDieuTri();
          } else {
            this.notificationService.showError(response.message || 'Cập nhật thất bại.');
          }
        },
        error: (err: HttpErrorResponse) => this.xuLyLoi(err)
      });
    } else {
      this.ketQuaDieuTriService.createService(ketQua).subscribe({
        next: (response: ApiResponse<IKetQuaDieuTri>) => {
          if (response.status) {
            this.notificationService.showSuccess('Thêm kết quả điều trị thành công!');
            this.xemChiTietHoSo(this.chiTietHoSo!.maHoSoYTe);
            this.huyFormKetQuaDieuTri();
          } else {
            this.notificationService.showError(response.message || 'Thêm thất bại.');
          }
        },
        error: (err: HttpErrorResponse) => this.xuLyLoi(err)
      });
    }
  }

  xoaKetQuaDieuTri(ketQua: IKetQuaDieuTri): void {
    if (confirm('Bạn có chắc chắn muốn xóa kết quả điều trị này?')) {
      this.ketQuaDieuTriService.deleteService(ketQua.maKetQuaDieuTri!).subscribe({
        next: () => {
          this.notificationService.showSuccess('Xóa kết quả điều trị thành công!');
          this.xemChiTietHoSo(this.chiTietHoSo!.maHoSoYTe);
        },
        error: (err: HttpErrorResponse) => this.xuLyLoi(err)
      });
    }
  }

  huyFormKetQuaDieuTri(): void {
    this.hienFormKetQuaDieuTri = false;
    this.dangSuaKetQuaDieuTri = false;
    this.ketQuaDieuTriDangChon = null;
    this.formKetQuaDieuTri.reset();
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
  
    yPosition += logoHeight + 2;
    
    return { doc, pageWidth, margin, yPosition };
  }

  private drawLine(doc: jsPDF, margin: number, yPosition: number, pageWidth: number): number {
    doc.setLineWidth(0.5);
    doc.line(margin, yPosition, pageWidth - margin, yPosition);
    return yPosition + 10;
  }

  printPrescription(): void {
    if (!this.chiTietHoSo || !this.chiTietHoSo.donThuoc || this.chiTietHoSo.donThuoc.length === 0) {
      this.notificationService.showError('Không có đơn thuốc để in.');
      return;
    }

    const { doc, pageWidth, margin, yPosition: initialY } = this.initializePDF('ĐƠN THUỐC');
    let yPosition = initialY;

    // Thông tin
    doc.setFontSize(12);
    doc.setFont('Roboto', 'normal');
    doc.text(`Mã hồ sơ y tế: ${this.chiTietHoSo.maHoSoYTe}`, margin, yPosition);
    yPosition += 6;
    doc.text(`Bệnh nhân: ${this.getTenBenhNhan(this.chiTietHoSo.maBenhNhan)}`, margin, yPosition);
    yPosition += 6;
    doc.text(`Chuẩn đoán: ${this.chiTietHoSo.chuanDoan}`, margin, yPosition);
    yPosition += 6;
    yPosition = this.drawLine(doc, margin, yPosition, pageWidth);

    // Tiêu đề bảng thuốc
    doc.setFontSize(14);
    doc.setFont('Roboto', 'bold');
    doc.text('Chi tiết thuốc', margin, yPosition);
    yPosition += 8;
    doc.setFont('Roboto', 'normal');

    // Vẽ bảng thuốc
    this.chiTietHoSo.donThuoc.forEach((donThuoc) => {
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

    doc.save(`DonThuoc_HoSo_${this.chiTietHoSo.maHoSoYTe}.pdf`);
  }

  printMedicalRecordDetail(): void {
    if (!this.chiTietHoSo) {
      this.notificationService.showError('Không có hồ sơ y tế để in.');
      return;
    }

    const { doc, pageWidth, margin, yPosition: initialY } = this.initializePDF('CHI TIẾT HỒ SƠ Y TẾ');
    let yPosition = initialY;

    // Thông tin cơ bản
    doc.setFontSize(12);
    doc.setFont('Roboto', 'normal');
    doc.text(`Mã hồ sơ y tế: ${this.chiTietHoSo.maHoSoYTe}`, margin, yPosition);
    yPosition += 6;
    doc.text(`Bệnh nhân: ${this.getTenBenhNhan(this.chiTietHoSo.maBenhNhan)}`, margin, yPosition);
    yPosition += 6;
    doc.text(`Chuẩn đoán: ${this.chiTietHoSo.chuanDoan}`, margin, yPosition);
    yPosition += 6;
    doc.text(`Phương pháp điều trị: ${this.chiTietHoSo.phuongPhapDieuTri || 'Không có'}`, margin, yPosition);
    yPosition += 6;
    doc.text(`Lịch sử bệnh: ${this.chiTietHoSo.lichSuBenh || 'Không có'}`, margin, yPosition);
    yPosition += 6;
    yPosition = this.drawLine(doc, margin, yPosition, pageWidth);

    // 1. Triệu chứng
    doc.setFontSize(14);
    doc.setFont('Roboto', 'bold');
    doc.text('Triệu chứng', margin, yPosition);
    yPosition += 8;
    doc.setFont('Roboto', 'normal');
    if (this.chiTietHoSo.trieuChung && this.chiTietHoSo.trieuChung.length > 0) {
      autoTable(doc, {
        startY: yPosition,
        head: [['Tên triệu chứng', 'Mô tả', 'Thời gian xuất hiện']],
        body: this.chiTietHoSo.trieuChung.map((tc) => [
          tc.tenTrieuChung,
          tc.moTa || 'Không có',
          new Date(tc.thoiGianXuatHien).toLocaleString('vi-VN')
        ]),
        styles: { font: 'Roboto', fontSize: 10 },
        headStyles: { fillColor: [0, 102, 204], textColor: [255, 255, 255] },
        margin: { left: margin, right: margin },
        columnStyles: { 0: { cellWidth: 60 }, 1: { cellWidth: 80 }, 2: { cellWidth: 50 } }
      });
      yPosition = (doc as any).lastAutoTable.finalY + 10;
    } else {
      doc.setFontSize(10);
      doc.text('Không có triệu chứng.', margin, yPosition);
      yPosition += 10;
    }

    // 2. Kết quả xét nghiệm
    doc.setFontSize(14);
    doc.setFont('Roboto', 'bold');
    doc.text('Kết quả xét nghiệm', margin, yPosition);
    yPosition += 8;
    doc.setFont('Roboto', 'normal');
    if (this.chiTietHoSo.ketQuaXetNghiem && this.chiTietHoSo.ketQuaXetNghiem.length > 0) {
      autoTable(doc, {
        startY: yPosition,
        head: [['Tên xét nghiệm', 'Kết quả', 'Ngày xét nghiệm']],
        body: this.chiTietHoSo.ketQuaXetNghiem.map((kq) => [
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
    } else {
      doc.setFontSize(10);
      doc.text('Không có kết quả xét nghiệm.', margin, yPosition);
      yPosition += 10;
    }

    // 3. Đơn thuốc
    doc.setFontSize(14);
    doc.setFont('Roboto', 'bold');
    doc.text('Đơn thuốc', margin, yPosition);
    yPosition += 8;
    doc.setFont('Roboto', 'normal');
    if (this.chiTietHoSo.donThuoc && this.chiTietHoSo.donThuoc.length > 0) {
      this.chiTietHoSo.donThuoc.forEach((donThuoc) => {
        doc.setFontSize(10);
        doc.text(`Ngày kê đơn: ${new Date(donThuoc.ngayKeDon).toLocaleString('vi-VN')}`, margin, yPosition);
        yPosition += 6;

        autoTable(doc, {
          startY: yPosition,
          head: [['Tên thuốc', 'Số lượng', 'Cách dùng', 'Liều lượng', 'Tần suất', 'Thành tiền']],
          body: donThuoc.chiTietThuocList.map((thuoc) => [
            this.getTenThuoc(thuoc.maThuoc), // Sử dụng getTenThuoc thay vì hardcode
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
    } else {
      doc.setFontSize(10);
      doc.text('Không có đơn thuốc.', margin, yPosition);
      yPosition += 10;
    }

    // 4. Kết quả điều trị
    doc.setFontSize(14);
    doc.setFont('Roboto', 'bold');
    doc.text('Kết quả điều trị', margin, yPosition);
    yPosition += 8;
    doc.setFont('Roboto', 'normal');
    if (this.chiTietHoSo.ketQuaDieuTri && this.chiTietHoSo.ketQuaDieuTri.length > 0) {
      autoTable(doc, {
        startY: yPosition,
        head: [['Hiệu quả', 'Tác dụng phụ', 'Ngày đánh giá']],
        body: this.chiTietHoSo.ketQuaDieuTri.map((kq) => [
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
    } else {
      doc.setFontSize(10);
      doc.text('Không có kết quả điều trị.', margin, yPosition);
      yPosition += 10;
    }

    // Ghi chú và chữ ký
    doc.setFontSize(10);
    doc.text('Ghi chú: Thông tin trên được trích từ hệ thống quản lý phòng khám.', margin, yPosition);
    yPosition += 10;
    doc.setFont('Roboto', 'bold');
    doc.text('Bác sĩ phụ trách', pageWidth - margin - 30, yPosition);
    doc.line(pageWidth - margin - 30, yPosition + 2, pageWidth - margin, yPosition + 2);
    doc.setFont('Roboto', 'normal');

    doc.save(`HoSoYTe_ChiTiet_${this.chiTietHoSo.maHoSoYTe}.pdf`);
  }
}