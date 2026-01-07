import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListingItemForm } from './listing-item-form';

describe('ListingItemForm', () => {
  let component: ListingItemForm;
  let fixture: ComponentFixture<ListingItemForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListingItemForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListingItemForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
