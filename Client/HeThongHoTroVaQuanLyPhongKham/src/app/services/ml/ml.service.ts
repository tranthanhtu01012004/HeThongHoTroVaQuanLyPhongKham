import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../commons/ApiResponse';
import { BaseApiService } from '../base-api-service/base-api.service';
import { HttpClient } from '@angular/common/http';
import { IPrediction } from '../../interfaces/ml/IPrediction';

@Injectable({
  providedIn: 'root'
})
export class MLService extends BaseApiService {
  private endpoint = '/admin/prediction/predict';

  constructor(http: HttpClient) {
    super(http);
  }

  predictDiagnosis(symptoms: string[]): Observable<ApiResponse<IPrediction>> {
    const request = { symptoms };
    return this.http.post<ApiResponse<IPrediction>>(`${this.apiBaseUrl}${this.endpoint}`, request);
  }
}
