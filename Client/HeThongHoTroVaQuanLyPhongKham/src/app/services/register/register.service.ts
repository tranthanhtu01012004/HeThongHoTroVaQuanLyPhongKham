import { Injectable } from '@angular/core';
import { BaseApiService } from '../base-api-service/base-api.service';
import { HttpClient } from '@angular/common/http';
import { ILoginInformation } from '../../interfaces/Auth/ILoginInformation';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../commons/ApiResponse';
import { ILogin } from '../../interfaces/login/ILogin';

@Injectable({
  providedIn: 'root'
})
export class RegisterService extends BaseApiService {
  private endpoint = '/auth/register';

  constructor(http: HttpClient) {
    super(http);
  }

  register(register: ILoginInformation): Observable<ApiResponse<ILogin>> {
    return this.http.post<ApiResponse<ILogin>>(`${this.apiBaseUrl}${this.endpoint}`, register);
  }
}
