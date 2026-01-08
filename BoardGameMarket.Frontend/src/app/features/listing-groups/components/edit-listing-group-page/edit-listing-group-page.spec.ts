import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditListingGroupPage } from './edit-listing-group-page';

describe('EditListingGroup', () => {
  let component: EditListingGroupPage;
  let fixture: ComponentFixture<EditListingGroupPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditListingGroupPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditListingGroupPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
