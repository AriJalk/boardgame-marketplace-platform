import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListingItemCard } from './listing-item-card';

describe('ListingItemCard', () => {
  let component: ListingItemCard;
  let fixture: ComponentFixture<ListingItemCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListingItemCard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListingItemCard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
