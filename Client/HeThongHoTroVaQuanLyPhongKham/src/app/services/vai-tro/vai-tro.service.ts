import { Injectable } from '@angular/core';
import { BaseApiService } from '../base-api-service/base-api.service';
import { HttpClient } from '@angular/common/http';
import { IVaiTro } from '../../interfaces/vai-tro/IVaiTro';
import { ApiResponse } from '../../commons/ApiResponse';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class VaiTroService extends BaseApiService {

    private endpoint = '/admin/roles';
    
    constructor(http: HttpClient) {
      super(http);
    }
    getAllServices(): Observable<ApiResponse<IVaiTro[]>> {
      return this.http.get<ApiResponse<IVaiTro[]>>(`${this.apiBaseUrl}${this.endpoint}`);
    }

    GetById(id: number): Observable<ApiResponse<IVaiTro>> {
      return this.http.get<ApiResponse<IVaiTro>>(`${this.apiBaseUrl}${this.endpoint}/${id}`);
    }

    createService(service: IVaiTro): Observable<ApiResponse<IVaiTro>> {
      return this.http.post<ApiResponse<IVaiTro>>(`${this.apiBaseUrl}${this.endpoint}`, service);
    }
  
    updateService(id: number, service: IVaiTro): Observable<ApiResponse<IVaiTro>> {
      return this.http.put<ApiResponse<IVaiTro>>(`${this.apiBaseUrl}${this.endpoint}/${id}`, service);
    }
  
    deleteService(id: number): Observable<ApiResponse<any>> {
      return this.http.delete<ApiResponse<any>>(`${this.apiBaseUrl}${this.endpoint}/${id}`);
    }
}
