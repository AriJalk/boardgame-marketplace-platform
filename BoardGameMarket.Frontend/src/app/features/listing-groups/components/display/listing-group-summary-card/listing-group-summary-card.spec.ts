import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListingGroupSummaryCard } from './listing-group-summary-card';

describe('ListingGroupCard', () => {
  let component: ListingGroupSummaryCard;
  let fixture: ComponentFixture<ListingGroupSummaryCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListingGroupSummaryCard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListingGroupSummaryCard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
