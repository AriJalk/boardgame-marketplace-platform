import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QueueEntryCard } from './queue-entry-card';

describe('QueueEntryCard', () => {
  let component: QueueEntryCard;
  let fixture: ComponentFixture<QueueEntryCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QueueEntryCard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QueueEntryCard);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
