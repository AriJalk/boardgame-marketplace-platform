import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BuyerSubscriptionsPage } from './buyer-subscriptions-page';

describe('BuyerSubscriptionsPage', () => {
  let component: BuyerSubscriptionsPage;
  let fixture: ComponentFixture<BuyerSubscriptionsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BuyerSubscriptionsPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BuyerSubscriptionsPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
