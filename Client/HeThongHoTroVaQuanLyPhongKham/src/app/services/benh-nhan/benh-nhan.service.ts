import { Injectable } from '@angular/core';
import { BaseApiService } from '../base-api-service/base-api.service';
import { HttpClient } from '@angular/common/http';
import { IBenhNhan } from '../../interfaces/benh-nhan/IBenhNhan';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../commons/ApiResponse';

@Injectable({
  providedIn: 'root'
})
export class BenhNhanService extends BaseApiService {
  private endpoint = '/patients';
  
  constructor(http: HttpClient) {
    super(http);
  }
  // Lấy thông tin bệnh nhân dựa trên maTaiKhoan
  getBenhNhanByMaTaiKhoan(id: number): Observable<ApiResponse<IBenhNhan>> {
    return this.http.get<ApiResponse<IBenhNhan>>(`${this.apiBaseUrl}${this.endpoint}/by-tai-khoan/${id}`);
  }

  getBenhNhanByName(name: string): Observable<ApiResponse<IBenhNhan[]>> {
    return this.http.get<ApiResponse<IBenhNhan[]>>(`${this.apiBaseUrl}${this.endpoint}/by-name?name=${encodeURIComponent(name)}`);
  }
  
  updateForTen(id: number, benhNhan: IBenhNhan): Observable<ApiResponse<IBenhNhan>> {
    return this.http.patch<ApiResponse<IBenhNhan>>(`${this.apiBaseUrl}${this.endpoint}/${id}/name`, benhNhan);
  }

  getAll(page: number = 1, pageSize: number = 10): Observable<ApiResponse<IBenhNhan[]>> {
    return this.http.get<ApiResponse<IBenhNhan[]>>(`${this.apiBaseUrl}${this.endpoint}?page=${page}&pageSize=${pageSize}`);
  }
}
