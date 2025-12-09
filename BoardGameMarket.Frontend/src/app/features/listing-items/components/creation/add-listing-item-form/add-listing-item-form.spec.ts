import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddListingItemForm } from './add-listing-item-form';

describe('AddListingItemForm', () => {
  let component: AddListingItemForm;
  let fixture: ComponentFixture<AddListingItemForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddListingItemForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddListingItemForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
