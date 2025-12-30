import { TestBed } from '@angular/core/testing';

import { AuthStubService } from './auth-stub.service';

describe('AuthStubService', () => {
  let service: AuthStubService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthStubService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
