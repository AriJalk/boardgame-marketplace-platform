import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddListingGroupPage } from './add-listing-group-page';

describe('AddListingGroupPage', () => {
  let component: AddListingGroupPage;
  let fixture: ComponentFixture<AddListingGroupPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddListingGroupPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddListingGroupPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
