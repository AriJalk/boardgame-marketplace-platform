import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { AuthStubService } from '../../../core/auth/auth-stub.service';
import { API_CONFIG_SERVICE } from '../../../core/api-config/api-config.service';
import { ApiConfig } from '../../../core/api-config/api-config.interface';
import { Observable } from 'rxjs';
import { UserSummary } from '../../../core/users/models/user.model';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  constructor(
    private httpClient: HttpClient,
    private authService: AuthStubService,
    @Inject(API_CONFIG_SERVICE) private apiConfig: ApiConfig,
  ) {}

  getContactInfo(userId: string): Observable<string> {
    return this.httpClient.get(`${this.apiConfig.apiEndpoint}/users/${userId}/contact-email`, {
      headers: this.authService.headers,
      responseType: 'text',
    });
  }

  getUserSummary(userId: string): Observable<UserSummary> {
    return this.httpClient.get<UserSummary>(`${this.apiConfig.apiEndpoint}/users/${userId}`);
  }
}
