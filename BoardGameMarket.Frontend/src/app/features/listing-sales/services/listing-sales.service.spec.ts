import { TestBed } from '@angular/core/testing';

import { ListingSalesService } from './listing-sales.service';

describe('ListingSalesService', () => {
  let service: ListingSalesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ListingSalesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
