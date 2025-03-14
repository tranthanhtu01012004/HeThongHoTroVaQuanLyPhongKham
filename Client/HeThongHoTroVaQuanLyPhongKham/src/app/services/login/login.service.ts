import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../commons/ApiResponse';
import { HttpClient } from '@angular/common/http';
import { ILoginInformation } from '../../interfaces/Auth/ILoginInformation';
import { ILoginService } from '../../interfaces/Auth/ILoginService';
import { ILogin } from '../../interfaces/login/ILogin';
import { BaseApiService } from '../base-api-service/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class LoginService extends BaseApiService implements ILoginService {
  private endpoint = '/auth/login';

  constructor(http: HttpClient) {
    super(http);
  }

  login(login: ILoginInformation): Observable<ApiResponse<ILogin>> {
    return this.http.post<ApiResponse<ILogin>>(`${this.apiBaseUrl}${this.endpoint}`, login);
  }
}
