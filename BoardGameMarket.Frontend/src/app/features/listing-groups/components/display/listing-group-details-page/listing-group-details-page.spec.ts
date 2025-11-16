import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListingGroupDetailsPage } from './listing-group-details-page';

describe('ListingGroupDetailsPage', () => {
  let component: ListingGroupDetailsPage;
  let fixture: ComponentFixture<ListingGroupDetailsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListingGroupDetailsPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListingGroupDetailsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
