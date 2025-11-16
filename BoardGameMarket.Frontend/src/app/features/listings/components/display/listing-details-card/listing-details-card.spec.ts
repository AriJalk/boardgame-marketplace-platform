import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListingDetailsCard } from './listing-details-card';

describe('ListingDetailsCard', () => {
  let component: ListingDetailsCard;
  let fixture: ComponentFixture<ListingDetailsCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListingDetailsCard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListingDetailsCard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
