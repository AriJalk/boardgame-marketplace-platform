import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditListingGroup } from './edit-listing-group';

describe('EditListingGroup', () => {
  let component: EditListingGroup;
  let fixture: ComponentFixture<EditListingGroup>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditListingGroup]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditListingGroup);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
