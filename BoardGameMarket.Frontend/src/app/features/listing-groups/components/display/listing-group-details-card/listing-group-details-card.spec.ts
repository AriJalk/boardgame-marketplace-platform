import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListingGroupDetailsCard } from './listing-group-details-card';

describe('ListingGroupDetailsCard', () => {
  let component: ListingGroupDetailsCard;
  let fixture: ComponentFixture<ListingGroupDetailsCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListingGroupDetailsCard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListingGroupDetailsCard);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
