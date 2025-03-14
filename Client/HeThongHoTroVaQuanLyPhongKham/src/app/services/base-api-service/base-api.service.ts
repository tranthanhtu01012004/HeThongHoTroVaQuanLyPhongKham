import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BaseApiService {
  protected apiBaseUrl = environment.apiBaseUrl;

  constructor(protected http: HttpClient) {}
}
