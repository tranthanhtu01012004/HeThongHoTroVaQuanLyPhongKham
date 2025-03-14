import { Injectable } from '@angular/core';
import { IPhongKham } from '../../interfaces/phong-kham/IPhongKham';
import { ApiResponse } from '../../commons/ApiResponse';
import { HttpClient } from '@angular/common/http';
import { BaseApiService } from '../base-api-service/base-api.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PhongKhamService extends BaseApiService {

    private endpoint = '/admin/clinics';
    
    constructor(http: HttpClient) {
      super(http);
    }
    getAllServices(page: number = 1, pageSize: number = 10): Observable<ApiResponse<IPhongKham[]>> {
      return this.http.get<ApiResponse<IPhongKham[]>>(`${this.apiBaseUrl}${this.endpoint}?page=${page}&pageSize=${pageSize}`);
    }
  
    getServices(page: number = 1, pageSize: number = 10): Observable<ApiResponse<IPhongKham[]>> {
      return this.http.get<ApiResponse<IPhongKham[]>>(`${this.apiBaseUrl}${this.endpoint}?page=${page}&pageSize=${pageSize}`);
    }
  
    createService(service: IPhongKham): Observable<ApiResponse<IPhongKham>> {
      return this.http.post<ApiResponse<IPhongKham>>(`${this.apiBaseUrl}${this.endpoint}`, service);
    }
  
    updateService(id: number, service: IPhongKham): Observable<ApiResponse<IPhongKham>> {
      return this.http.put<ApiResponse<IPhongKham>>(`${this.apiBaseUrl}${this.endpoint}/${id}`, service);
    }
  
    deleteService(id: number): Observable<ApiResponse<any>> {
      return this.http.delete<ApiResponse<any>>(`${this.apiBaseUrl}${this.endpoint}/${id}`);
    }
}
