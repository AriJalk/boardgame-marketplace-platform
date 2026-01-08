import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateListingGroupPage } from './create-listing-group-page';

describe('CreateListingGroupPage', () => {
  let component: CreateListingGroupPage;
  let fixture: ComponentFixture<CreateListingGroupPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateListingGroupPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateListingGroupPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
