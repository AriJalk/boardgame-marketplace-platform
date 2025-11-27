import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddBggListingItemForm } from './add-bgg-listing-item-form';

describe('AddBggListingItemForm', () => {
  let component: AddBggListingItemForm;
  let fixture: ComponentFixture<AddBggListingItemForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddBggListingItemForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddBggListingItemForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
