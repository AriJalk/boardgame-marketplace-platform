import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListingGroupCreationPage } from './listing-group-creation-page';

describe('ListingGroupCreationPage', () => {
  let component: ListingGroupCreationPage;
  let fixture: ComponentFixture<ListingGroupCreationPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListingGroupCreationPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListingGroupCreationPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
