import { Injectable } from '@angular/core';
import { BaseApiService } from '../base-api-service/base-api.service';
import { HttpClient } from '@angular/common/http';
import { ITrieuChung } from '../../interfaces/trieu-chung/ITrieuChung';
import { ApiResponse } from '../../commons/ApiResponse';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TrieuChungService extends BaseApiService {
  private endpoint = '/admin/symptoms';
  
  constructor(http: HttpClient) {
    super(http);
  }

  getAllServices(page: number = 1, pageSize: number = 10): Observable<ApiResponse<ITrieuChung[]>> {
    return this.http.get<ApiResponse<ITrieuChung[]>>(`${this.apiBaseUrl}${this.endpoint}?page=${page}&pageSize=${pageSize}`);
  }

  createService(service: ITrieuChung): Observable<ApiResponse<ITrieuChung>> {
    return this.http.post<ApiResponse<ITrieuChung>>(`${this.apiBaseUrl}${this.endpoint}`, service);
  }

  updateService(id: number, service: ITrieuChung): Observable<ApiResponse<ITrieuChung>> {
    return this.http.put<ApiResponse<ITrieuChung>>(`${this.apiBaseUrl}${this.endpoint}/${id}`, service);
  }

  deleteService(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiBaseUrl}${this.endpoint}/${id}`);
  }
}
