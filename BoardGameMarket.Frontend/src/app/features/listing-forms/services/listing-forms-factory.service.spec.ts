import { TestBed } from '@angular/core/testing';

import { ListingFormsFactoryService } from './listing-forms-factory.service';

describe('ListingFormsFactory', () => {
  let service: ListingFormsFactoryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ListingFormsFactoryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
