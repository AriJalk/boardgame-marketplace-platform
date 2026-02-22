import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserListingsPage } from './user-listings-page';

describe('UserListingsPage', () => {
  let component: UserListingsPage;
  let fixture: ComponentFixture<UserListingsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserListingsPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserListingsPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
