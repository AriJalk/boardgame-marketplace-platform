import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListingSummaryCard } from './listing-summary-card';

describe('ListingCard', () => {
  let component: ListingSummaryCard;
  let fixture: ComponentFixture<ListingSummaryCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListingSummaryCard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListingSummaryCard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
