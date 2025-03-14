import { Injectable } from '@angular/core';
import { BaseApiService } from '../base-api-service/base-api.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../commons/ApiResponse';
import { ILichHenUpdate } from '../../interfaces/lich-hen/ILichHenUpdate';
import { environment } from '../../environments/environment';
import { ILichHen } from '../../interfaces/lich-hen/ILichHen';

@Injectable({
  providedIn: 'root'
})
export class LichHenService extends BaseApiService {
  private apiUrl = `${environment.apiBaseUrl}/api/admin/appointments`;

  constructor(http: HttpClient) {
    super(http);
  }

  // Lấy danh sách lịch hẹn với phân trang và lọc
  getAll(page: number, pageSize: number, ngayHen?: Date, maNhanVien?: number, maPhong?: number): Observable<ApiResponse<ILichHen[]>> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    if (ngayHen) {
      params = params.set('ngayHen', ngayHen.toISOString());
    }
    if (maNhanVien) {
      params = params.set('maNhanVien', maNhanVien.toString());
    }
    if (maPhong) {
      params = params.set('maPhong', maPhong.toString());
    }

    return this.http.get<ApiResponse<ILichHen[]>>(this.apiUrl, { params });
  }

  // Lấy lịch hẹn theo ID
  getById(id: number): Observable<ApiResponse<ILichHen>> {
    return this.http.get<ApiResponse<ILichHen>>(`${this.apiUrl}/${id}`);
  }

  // Thêm lịch hẹn mới
  add(lichHen: ILichHen): Observable<ApiResponse<ILichHen>> {
    return this.http.post<ApiResponse<ILichHen>>(this.apiUrl, lichHen);
  }

  // Cập nhật lịch hẹn
  update(id: number, lichHen: ILichHen): Observable<ApiResponse<ILichHen>> {
    return this.http.put<ApiResponse<ILichHen>>(`${this.apiUrl}/${id}`, lichHen);
  }

  // Cập nhật trạng thái lịch hẹn
  updateTrangThai(id: number, updateDto: ILichHenUpdate): Observable<ApiResponse<ILichHen>> {
    return this.http.patch<ApiResponse<ILichHen>>(`${this.apiUrl}/${id}/status`, updateDto);
  }

  // Xóa lịch hẹn
  delete(id: number): Observable<ApiResponse<void>> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/${id}`);
  }
}
