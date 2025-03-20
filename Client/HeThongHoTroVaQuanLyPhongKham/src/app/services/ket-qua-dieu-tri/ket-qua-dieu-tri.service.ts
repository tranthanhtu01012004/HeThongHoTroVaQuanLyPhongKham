import { Injectable } from '@angular/core';
import { BaseApiService } from '../base-api-service/base-api.service';
import { HttpClient } from '@angular/common/http';
import { IKetQuaDieuTri } from '../../interfaces/ket-qua-deu-tri/IKetQuaDieuTri';
import { ApiResponse } from '../../commons/ApiResponse';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class KetQuaDieuTriService extends BaseApiService {
  private endpoint = '/admin/treatment-outcomes';
  
  constructor(http: HttpClient) {
    super(http);
  }

  getAllServices(page: number = 1, pageSize: number = 10): Observable<ApiResponse<IKetQuaDieuTri[]>> {
    return this.http.get<ApiResponse<IKetQuaDieuTri[]>>(`${this.apiBaseUrl}${this.endpoint}?page=${page}&pageSize=${pageSize}`);
  }

  createService(service: IKetQuaDieuTri): Observable<ApiResponse<IKetQuaDieuTri>> {
    return this.http.post<ApiResponse<IKetQuaDieuTri>>(`${this.apiBaseUrl}${this.endpoint}`, service);
  }

  updateService(id: number, service: IKetQuaDieuTri): Observable<ApiResponse<IKetQuaDieuTri>> {
    return this.http.put<ApiResponse<IKetQuaDieuTri>>(`${this.apiBaseUrl}${this.endpoint}/${id}`, service);
  }

  deleteService(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiBaseUrl}${this.endpoint}/${id}`);
  }
}
