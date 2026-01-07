import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RoleType, UserLogin, UserSummary } from '../users/models/user.model';

@Injectable({
  providedIn: 'root',
})
export class AuthStubService {
  loggedInUser?: UserLogin;

  sellerId = '4552ea90-84c8-4d7c-8409-d80b39cad771';
  buyerId = '4552ea90-84c8-4d7c-8409-d80b39cad772';
  constructor(private httpClient: HttpClient) {
    this.loggedInUser = {
      id: this.sellerId,
      displayName: 'Test user',
      token: 'bearer TEST_TOKEN',
      role: RoleType.User,
    };
  }
}
