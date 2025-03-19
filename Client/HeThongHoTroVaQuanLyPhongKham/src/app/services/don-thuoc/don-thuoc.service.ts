import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../base-api-service/base-api.service';
import { IDonThuoc } from '../../interfaces/don-thuoc/IDonThuoc';
import { ApiResponse } from '../../commons/ApiResponse';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DonThuocService extends BaseApiService {
  private endpoint = '/admin/prescriptions';
  
  constructor(http: HttpClient) {
    super(http);
  }

  getAllServices(page: number = 1, pageSize: number = 10): Observable<ApiResponse<IDonThuoc[]>> {
    return this.http.get<ApiResponse<IDonThuoc[]>>(`${this.apiBaseUrl}${this.endpoint}?page=${page}&pageSize=${pageSize}`);
  }

  createService(service: IDonThuoc): Observable<ApiResponse<IDonThuoc>> {
    return this.http.post<ApiResponse<IDonThuoc>>(`${this.apiBaseUrl}${this.endpoint}`, service);
  }

  updateService(id: number, service: IDonThuoc): Observable<ApiResponse<IDonThuoc>> {
    return this.http.put<ApiResponse<IDonThuoc>>(`${this.apiBaseUrl}${this.endpoint}/${id}`, service);
  }

  deleteService(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiBaseUrl}${this.endpoint}/${id}`);
  }
}

