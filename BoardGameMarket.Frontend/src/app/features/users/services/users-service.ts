import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, OnInit } from '@angular/core';
import { API_CONFIG_SERVICE } from '../../../core/api-config/api-config.service';
import { ApiConfig } from '../../../core/api-config/api-config.interface';
import {
  BehaviorSubject,
  from,
  Observable,
  of,
  shareReplay,
  startWith,
  switchMap,
  tap,
} from 'rxjs';
import { UserSummary } from '../../../core/users/models/user.model';
import { AuthService } from '../../../core/auth/auth.service';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  constructor(
    private httpClient: HttpClient,
    @Inject(API_CONFIG_SERVICE) private apiConfig: ApiConfig,
  ) {}

  getContactInfo(userId: string, authHeader: string): Observable<string> {
    const headers = authHeader ? { Authorization: authHeader } : undefined;
    return this.httpClient.get(`${this.apiConfig.apiEndpoint}/users/${userId}/contact-email`, {
      headers: headers,
      responseType: 'text',
    });
  }

  getUserSummary(userId: string): Observable<UserSummary> {
    return this.httpClient.get<UserSummary>(`${this.apiConfig.apiEndpoint}/users/${userId}`);
  }
}
