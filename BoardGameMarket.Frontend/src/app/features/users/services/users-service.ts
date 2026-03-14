import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, OnInit } from '@angular/core';
import { API_CONFIG_SERVICE } from '../../../core/api-config/api-config.service';
import { ApiConfig } from '../../../core/api-config/api-config.interface';
import { BehaviorSubject, firstValueFrom, Observable, of } from 'rxjs';
import { UserSummary } from '../../../core/users/models/user.model';
import { AuthService } from '../../../core/auth/auth.service';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  
  private _loggedUser$ = new BehaviorSubject<UserSummary | undefined>(undefined);

  get loggedUser$(): Observable<UserSummary | undefined> {
    if (!this._loggedUser$.value && this.authService.activeAccount) {
      firstValueFrom(this.getUserSummary(this.authService.activeAccount.localAccountId)).then(
        (user) => this._loggedUser$.next(user),
      );
    }
    return this._loggedUser$.asObservable();
  }

  constructor(
    private httpClient: HttpClient,
    @Inject(API_CONFIG_SERVICE) private apiConfig: ApiConfig,
    private authService: AuthService,
  ) {}

  getContactInfo(userId: string): Observable<string> {
    return this.httpClient.get(`${this.apiConfig.apiEndpoint}/users/${userId}/contact-email`, {
      responseType: 'text',
    });
  }

  getUserSummary(userId: string): Observable<UserSummary> {
    return this.httpClient.get<UserSummary>(`${this.apiConfig.apiEndpoint}/users/${userId}`);
  }
}
