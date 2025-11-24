import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddListingGroupForm } from './add-listing-group-form';

describe('AddListingGroupForm', () => {
  let component: AddListingGroupForm;
  let fixture: ComponentFixture<AddListingGroupForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddListingGroupForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddListingGroupForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
