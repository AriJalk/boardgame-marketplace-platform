import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddListingForm } from './add-listing-form';

describe('AddListingForm', () => {
  let component: AddListingForm;
  let fixture: ComponentFixture<AddListingForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddListingForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddListingForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
