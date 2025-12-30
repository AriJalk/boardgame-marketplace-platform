import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QueueSellerPanel } from './queue-seller-panel';

describe('QueueSellerPanel', () => {
  let component: QueueSellerPanel;
  let fixture: ComponentFixture<QueueSellerPanel>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QueueSellerPanel]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QueueSellerPanel);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
