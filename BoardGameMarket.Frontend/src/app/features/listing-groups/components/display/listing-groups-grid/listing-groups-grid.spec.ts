import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListingGroupsGrid } from './listing-groups-grid';

describe('ListingGroupsGrid', () => {
  let component: ListingGroupsGrid;
  let fixture: ComponentFixture<ListingGroupsGrid>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListingGroupsGrid]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListingGroupsGrid);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
