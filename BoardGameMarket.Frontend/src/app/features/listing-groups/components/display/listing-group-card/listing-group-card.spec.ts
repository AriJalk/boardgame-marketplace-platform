import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListingGroupCard } from './listing-group-card';

describe('ListingGroupCard', () => {
  let component: ListingGroupCard;
  let fixture: ComponentFixture<ListingGroupCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListingGroupCard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListingGroupCard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
