import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListingGroupForm } from './listing-group-form';

describe('ListingGroupForm', () => {
  let component: ListingGroupForm;
  let fixture: ComponentFixture<ListingGroupForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListingGroupForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListingGroupForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
