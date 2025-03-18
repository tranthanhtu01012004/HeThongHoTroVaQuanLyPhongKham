import { Injectable } from '@angular/core';
import { BaseApiService } from '../base-api-service/base-api.service';
import { HttpClient } from '@angular/common/http';
import { IThuoc } from '../../interfaces/thuoc/IThuoc';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../commons/ApiResponse';

@Injectable({
  providedIn: 'root'
})
export class ThuocService extends BaseApiService{
  private endpoint = '/admin/medicines';
  
  constructor(http: HttpClient) {
    super(http);
  }

  getAllServices(page: number = 1, pageSize: number = 10): Observable<ApiResponse<IThuoc[]>> {
    return this.http.get<ApiResponse<IThuoc[]>>(`${this.apiBaseUrl}${this.endpoint}?page=${page}&pageSize=${pageSize}`);
  }

  createService(service: IThuoc): Observable<ApiResponse<IThuoc>> {
    return this.http.post<ApiResponse<IThuoc>>(`${this.apiBaseUrl}${this.endpoint}`, service);
  }

  updateService(id: number, service: IThuoc): Observable<ApiResponse<IThuoc>> {
    return this.http.put<ApiResponse<IThuoc>>(`${this.apiBaseUrl}${this.endpoint}/${id}`, service);
  }

  deleteService(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiBaseUrl}${this.endpoint}/${id}`);
  }
}
