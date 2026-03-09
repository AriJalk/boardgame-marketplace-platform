import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyListingsSalesPage } from './my-listing-sales-page';

describe('MyListingSalesPage', () => {
  let component: MyListingsSalesPage;
  let fixture: ComponentFixture<MyListingsSalesPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyListingsSalesPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyListingsSalesPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
