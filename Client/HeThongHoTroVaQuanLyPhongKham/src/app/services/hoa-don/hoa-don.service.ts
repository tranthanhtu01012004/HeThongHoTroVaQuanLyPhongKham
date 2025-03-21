import { Injectable } from '@angular/core';
import { BaseApiService } from '../base-api-service/base-api.service';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../../commons/ApiResponse';
import { Observable } from 'rxjs';
import { IHoaDon } from '../../interfaces/hoa-don/IHoaDon';

@Injectable({
  providedIn: 'root'
})
export class HoaDonService extends BaseApiService {

  private endpoint = '/admin/invoices';

  constructor(http: HttpClient) {
    super(http);
  }
  getAll(): Observable<ApiResponse<IHoaDon[]>> {
    return this.http.get<ApiResponse<IHoaDon[]>>(`${this.apiBaseUrl}${this.endpoint}?page=1&pageSize=1000`);
  }

  updateTrangThai(maHoaDon: number, hoaDon: IHoaDon): Observable<ApiResponse<IHoaDon>> {
    return this.http.patch<ApiResponse<IHoaDon>>(`${this.apiBaseUrl}${this.endpoint}/${maHoaDon}/status`, {
      maHoaDon: hoaDon.maHoaDon,
      trangThaiThanhToan: hoaDon.trangThaiThanhToan
    });
  }

  delete(maHoaDon: number): Observable<void> {
    return this.http.delete<void>(`${this.apiBaseUrl}${this.endpoint}/${maHoaDon}`);
  }
}
