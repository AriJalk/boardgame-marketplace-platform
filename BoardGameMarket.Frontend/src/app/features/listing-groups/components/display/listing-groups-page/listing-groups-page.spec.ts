import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListingGroupsPage } from './listing-groups-page';

describe('ListingGroupsPage', () => {
  let component: ListingGroupsPage;
  let fixture: ComponentFixture<ListingGroupsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListingGroupsPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListingGroupsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
