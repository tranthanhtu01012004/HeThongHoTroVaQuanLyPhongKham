import { Injectable } from '@angular/core';
import { BaseApiService } from '../base-api-service/base-api.service';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../../commons/ApiResponse';
import { IPhongKhamNhanVien } from '../../interfaces/phong-kham-nhan-vien/IPhongKhamNhanVien';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PhongKhamNhanVienService extends BaseApiService {

  private endpoint = '/admin/clinic-employees';
  
  constructor(http: HttpClient) {
    super(http);
  }
  
  getAllServices(page: number = 1, pageSize: number = 10): Observable<ApiResponse<IPhongKhamNhanVien[]>> {
    return this.http.get<ApiResponse<IPhongKhamNhanVien[]>>(`${this.apiBaseUrl}${this.endpoint}?page=${page}&pageSize=${pageSize}`);
  }

  getAllServicesNotPaginator(): Observable<ApiResponse<IPhongKhamNhanVien[]>> {
    return this.http.get<ApiResponse<IPhongKhamNhanVien[]>>(`${this.apiBaseUrl}${this.endpoint}`);
  }
  createService(service: IPhongKhamNhanVien): Observable<ApiResponse<IPhongKhamNhanVien>> {
    return this.http.post<ApiResponse<IPhongKhamNhanVien>>(`${this.apiBaseUrl}${this.endpoint}`, service);
  }

  updateService(id: number, service: IPhongKhamNhanVien): Observable<ApiResponse<IPhongKhamNhanVien>> {
    return this.http.put<ApiResponse<IPhongKhamNhanVien>>(`${this.apiBaseUrl}${this.endpoint}/${id}`, service);
  }

  deleteService(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiBaseUrl}${this.endpoint}/${id}`);
  }
}
