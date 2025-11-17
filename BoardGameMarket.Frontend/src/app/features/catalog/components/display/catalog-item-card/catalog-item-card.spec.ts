import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CatalogItemCard } from './catalog-item-card';

describe('CatalogItemCard', () => {
  let component: CatalogItemCard;
  let fixture: ComponentFixture<CatalogItemCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CatalogItemCard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CatalogItemCard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
