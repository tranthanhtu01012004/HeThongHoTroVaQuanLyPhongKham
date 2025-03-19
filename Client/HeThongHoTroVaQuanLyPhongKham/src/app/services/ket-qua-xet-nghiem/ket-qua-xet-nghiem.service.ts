import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from '../base-api-service/base-api.service';
import { IKetQuaXetNghiem } from '../../interfaces/ket-qua-xet-nghiem/IKetQuaXetNghiem';
import { ApiResponse } from '../../commons/ApiResponse';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class KetQuaXetNghiemService extends BaseApiService {
  private endpoint = '/admin/test-results';
  
  constructor(http: HttpClient) {
    super(http);
  }

  getAllServices(page: number = 1, pageSize: number = 10): Observable<ApiResponse<IKetQuaXetNghiem[]>> {
    return this.http.get<ApiResponse<IKetQuaXetNghiem[]>>(`${this.apiBaseUrl}${this.endpoint}?page=${page}&pageSize=${pageSize}`);
  }

  createService(service: IKetQuaXetNghiem): Observable<ApiResponse<IKetQuaXetNghiem>> {
    return this.http.post<ApiResponse<IKetQuaXetNghiem>>(`${this.apiBaseUrl}${this.endpoint}`, service);
  }

  updateService(id: number, service: IKetQuaXetNghiem): Observable<ApiResponse<IKetQuaXetNghiem>> {
    return this.http.put<ApiResponse<IKetQuaXetNghiem>>(`${this.apiBaseUrl}${this.endpoint}/${id}`, service);
  }

  deleteService(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiBaseUrl}${this.endpoint}/${id}`);
  }
}
