import { Injectable } from '@angular/core';
import { BaseApiService } from '../base-api-service/base-api.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../commons/ApiResponse';
import { INhanVien } from '../../interfaces/nhan-vien/INhanVien';

@Injectable({
  providedIn: 'root'
})
export class NhanVienService extends BaseApiService{

  private endpoint = '/admin/employees';
  
  constructor(http: HttpClient) {
    super(http);
  }
  getAllServices(page: number = 1, pageSize: number = 10): Observable<ApiResponse<INhanVien[]>> {
    return this.http.get<ApiResponse<INhanVien[]>>(`${this.apiBaseUrl}${this.endpoint}?page=${page}&pageSize=${pageSize}`);
  }

  getServices(page: number = 1, pageSize: number = 10): Observable<ApiResponse<INhanVien[]>> {
    return this.http.get<ApiResponse<INhanVien[]>>(`${this.apiBaseUrl}${this.endpoint}?page=${page}&pageSize=${pageSize}`);
  }

  createService(nhanVien: INhanVien): Observable<ApiResponse<INhanVien>> {
    return this.http.post<ApiResponse<INhanVien>>(`${this.apiBaseUrl}${this.endpoint}`, nhanVien);
  }

  updateService(id: number, nhanVien: INhanVien): Observable<ApiResponse<INhanVien>> {
    return this.http.put<ApiResponse<INhanVien>>(`${this.apiBaseUrl}${this.endpoint}/${id}`, nhanVien);
  }

  deleteService(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiBaseUrl}${this.endpoint}/${id}`);
  }
}
