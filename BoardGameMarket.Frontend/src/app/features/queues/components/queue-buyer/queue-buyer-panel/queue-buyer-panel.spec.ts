import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QueueBuyerPanel } from './queue-buyer-panel';

describe('QueueBuyerPanel', () => {
  let component: QueueBuyerPanel;
  let fixture: ComponentFixture<QueueBuyerPanel>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QueueBuyerPanel]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QueueBuyerPanel);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
