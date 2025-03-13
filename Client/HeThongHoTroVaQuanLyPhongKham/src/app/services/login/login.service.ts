import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../commons/ApiResponse';
import { HttpClient } from '@angular/common/http';
import { ILoginInformation } from '../../interfaces/Auth/ILoginInformation';
import { ILoginService } from '../../interfaces/Auth/ILoginService';
import { LoginData } from '../../responses/LoginData';

@Injectable({
  providedIn: 'root'
})
export class LoginService implements ILoginService {
  private apiUrl = 'https://localhost:7162/api/auth/login';

  constructor(private http: HttpClient) { }

  login(login: ILoginInformation): Observable<ApiResponse<LoginData>> {
    return this.http.post<ApiResponse<LoginData>>(this.apiUrl, login);
  }
}
