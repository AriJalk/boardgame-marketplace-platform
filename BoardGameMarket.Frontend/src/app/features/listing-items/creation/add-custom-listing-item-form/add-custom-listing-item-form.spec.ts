import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCustomListingItemForm } from './add-custom-listing-item-form';

describe('AddCustomListingItemForm', () => {
  let component: AddCustomListingItemForm;
  let fixture: ComponentFixture<AddCustomListingItemForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddCustomListingItemForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddCustomListingItemForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
