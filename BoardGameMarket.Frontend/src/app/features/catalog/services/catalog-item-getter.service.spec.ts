import { TestBed } from '@angular/core/testing';

import { CatalogItemGetterService } from './catalog-item-getter.service';

describe('CatalogItemGetterService', () => {
  let service: CatalogItemGetterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CatalogItemGetterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
