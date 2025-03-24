import { Injectable } from '@angular/core';
import { BaseApiService } from '../base-api-service/base-api.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../commons/ApiResponse';
import { IDichVuYTe } from '../../interfaces/dich-vu-y-te/IDichVuYTe';

@Injectable({
  providedIn: 'root'
})
export class DichVuYTeService extends BaseApiService {
  private endpoint = '/admin/healthcare-services';
  
  constructor(http: HttpClient) {
    super(http);
  }
  getAllServices(page: number = 1, pageSize: number = 10): Observable<ApiResponse<IDichVuYTe[]>> {
    return this.http.get<ApiResponse<IDichVuYTe[]>>(`${this.apiBaseUrl}${this.endpoint}?page=${page}&pageSize=${pageSize}`);
  }

  getServices(page: number = 1, pageSize: number = 10): Observable<ApiResponse<IDichVuYTe[]>> {
    return this.http.get<ApiResponse<IDichVuYTe[]>>(`${this.apiBaseUrl}${this.endpoint}?page=${page}&pageSize=${pageSize}`);
  }

  createService(service: IDichVuYTe): Observable<ApiResponse<IDichVuYTe>> {
    return this.http.post<ApiResponse<IDichVuYTe>>(`${this.apiBaseUrl}${this.endpoint}`, service);
  }

  updateService(id: number, service: IDichVuYTe): Observable<ApiResponse<IDichVuYTe>> {
    return this.http.put<ApiResponse<IDichVuYTe>>(`${this.apiBaseUrl}${this.endpoint}/${id}`, service);
  }

  deleteService(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiBaseUrl}${this.endpoint}/${id}`);
  }
}
