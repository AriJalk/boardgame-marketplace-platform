import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RoleType, UserLogin, UserSummary } from '../users/models/user.model';

@Injectable({
  providedIn: 'root',
})
export class AuthStubService {
  loggedInUser?: UserLogin;

  sellerId = 'c2631fac-ffb9-4c18-a690-9a363570f770';
  buyerId = '4552ea90-84c8-4d7c-8409-d80b39cad772';
  constructor(private httpClient: HttpClient) {
    this.loggedInUser = {
      userId: this.sellerId,
      displayName: 'Test user',
      token: 'bearer TEST_TOKEN',
      role: RoleType.User,
    };
  }
}
