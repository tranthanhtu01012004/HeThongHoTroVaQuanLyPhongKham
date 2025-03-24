import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../commons/ApiResponse';
import { IHoSoYTe } from '../../interfaces/ho-so-y-te/IHoSoYTe';
import { BaseApiService } from '../base-api-service/base-api.service';
import { HttpClient } from '@angular/common/http';
import { IHoSoYTeDetail } from '../../interfaces/ho-so-y-te/IHoSoYTeDetail';

@Injectable({
  providedIn: 'root'
})
export class HoSoYTeService extends BaseApiService {

  private endpoint = '/admin/medical-records';

    constructor(http: HttpClient) {
      super(http);
    }


  getMedicalDetailRecord(id: number): Observable<ApiResponse<IHoSoYTeDetail>> {
    return this.http.get<ApiResponse<IHoSoYTeDetail>>(`${this.apiBaseUrl}${this.endpoint}/${id}/detail`);
  }

  getAllMedicalRecords(page: number, pageSize: number): Observable<ApiResponse<IHoSoYTe[]>> {
    return this.http.get<ApiResponse<IHoSoYTe[]>>(`${this.apiBaseUrl}${this.endpoint}?page=${page}&pageSize=${pageSize}`);
  }

  getMedicalRecordById(id: number): Observable<ApiResponse<IHoSoYTe>> {
    return this.http.get<ApiResponse<IHoSoYTe>>(`${this.apiBaseUrl}${this.endpoint}/${id}`);
  }

  createMedicalRecord(record: IHoSoYTe): Observable<ApiResponse<IHoSoYTe>> {
    return this.http.post<ApiResponse<IHoSoYTe>>(`${this.apiBaseUrl}${this.endpoint}`, record);
  }

  updateMedicalRecord(id: number, record: IHoSoYTe): Observable<ApiResponse<IHoSoYTe>> {
    return this.http.put<ApiResponse<IHoSoYTe>>(`${this.apiBaseUrl}${this.endpoint}/${id}`, record);
  }

  deleteMedicalRecord(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiBaseUrl}${this.endpoint}/${id}`);
  }
}
