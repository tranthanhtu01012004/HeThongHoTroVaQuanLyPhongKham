import { Injectable } from '@angular/core';
import { BaseApiService } from '../base-api-service/base-api.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../commons/ApiResponse';
import { ILichHenUpdate } from '../../interfaces/lich-hen/ILichHenUpdate';
import { ILichHen } from '../../interfaces/lich-hen/ILichHen';
import { INhanVienLichHen } from '../../interfaces/lich-hen/INhanVienLichHen';

@Injectable({
  providedIn: 'root'
})
export class LichHenService extends BaseApiService {
  private endpoint = '/admin/appointments';

  constructor(http: HttpClient) {
    super(http);
  }

  // Lấy danh sách lịch hẹn với phân trang và lọc
  getAll(page: number, pageSize: number, ngayHen?: string, maNhanVien?: number, maPhong?: number): Observable<ApiResponse<ILichHen[]>> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

      if (ngayHen) {
        params = params.set('ngayHen', ngayHen);
      }
    if (maNhanVien) {
      params = params.set('maNhanVien', maNhanVien.toString());
    }
    if (maPhong) {
      params = params.set('maPhong', maPhong.toString());
    }

    return this.http.get<ApiResponse<ILichHen[]>>(`${this.apiBaseUrl}${this.endpoint}`, { params }); // Sửa this.apiUrl thành this.apiBaseUrl
  }

  // Lấy lịch hẹn theo ID
  getById(id: number): Observable<ApiResponse<ILichHen>> {
    return this.http.get<ApiResponse<ILichHen>>(`${this.apiBaseUrl}${this.endpoint}/${id}`);
  }

  getLichHenByMaBenhNhan(maBenhNhan: number): Observable<ApiResponse<ILichHen>> {
    return this.http.get<ApiResponse<ILichHen>>(`${this.apiBaseUrl}${this.endpoint}/patient/${maBenhNhan}`);
  }

  getLichHenByMaHoSoYTe(id: number): Observable<ApiResponse<any[]>> {
    return this.http.get<ApiResponse<any[]>>(`${this.apiBaseUrl}${this.endpoint}/${id}/lich-hen`);
  }

  // Thêm lịch hẹn mới
  add(lichHen: ILichHen): Observable<ApiResponse<ILichHen>> {
    return this.http.post<ApiResponse<ILichHen>>(`${this.apiBaseUrl}${this.endpoint}`, lichHen);
  }

  createForPatient(lichHen: any): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`${this.apiBaseUrl}${this.endpoint}/patient`, lichHen);
  }

  // Cập nhật lịch hẹn
  update(id: number, lichHen: INhanVienLichHen): Observable<ApiResponse<ILichHen>> {
    return this.http.patch<ApiResponse<ILichHen>>(`${this.apiBaseUrl}${this.endpoint}/${id}`, lichHen);
  }

  // Cập nhật trạng thái lịch hẹn
  updateTrangThai(id: number, updateDto: ILichHenUpdate): Observable<ApiResponse<ILichHen>> {
    return this.http.patch<ApiResponse<ILichHen>>(`${this.apiBaseUrl}${this.endpoint}/${id}/status`, updateDto);
  }

  // Xóa lịch hẹn
  delete(id: number): Observable<ApiResponse<void>> {
    return this.http.delete<ApiResponse<void>>(`${this.apiBaseUrl}${this.endpoint}/${id}`);
  }
}
