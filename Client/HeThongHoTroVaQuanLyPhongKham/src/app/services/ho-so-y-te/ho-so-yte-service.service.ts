import { Injectable } from '@angular/core';
import { BaseApiService } from '../base-api-service/base-api.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../commons/ApiResponse';
import { IHoSoYTe } from '../../interfaces/ho-so-y-te/IHoSoYTe';

@Injectable({
  providedIn: 'root'
})
export class HoSoYTeServiceService extends BaseApiService {
  private apiUrl = '/admin/medical-records';

  constructor(http: HttpClient) {
    super(http);
  }

  getAllMedicalRecords(page: number, pageSize: number): Observable<ApiResponse<IHoSoYTe[]>> {
    return this.http.get<ApiResponse<IHoSoYTe[]>>(`${this.apiUrl}?page=${page}&pageSize=${pageSize}`);
  }

  getMedicalRecordById(id: number): Observable<ApiResponse<IHoSoYTe>> {
    return this.http.get<ApiResponse<IHoSoYTe>>(`${this.apiUrl}/${id}`);
  }

  createMedicalRecord(record: IHoSoYTe): Observable<ApiResponse<IHoSoYTe>> {
    return this.http.post<ApiResponse<IHoSoYTe>>(this.apiUrl, record);
  }

  updateMedicalRecord(id: number, record: IHoSoYTe): Observable<ApiResponse<IHoSoYTe>> {
    return this.http.put<ApiResponse<IHoSoYTe>>(`${this.apiUrl}/${id}`, record);
  }

  deleteMedicalRecord(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
