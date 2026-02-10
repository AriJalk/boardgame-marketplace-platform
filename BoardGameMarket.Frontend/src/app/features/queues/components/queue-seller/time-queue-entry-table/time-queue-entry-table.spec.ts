import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TimeQueueEntryTable } from './time-queue-entry-table';

describe('TimeQueueEntryTable', () => {
  let component: TimeQueueEntryTable;
  let fixture: ComponentFixture<TimeQueueEntryTable>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TimeQueueEntryTable]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TimeQueueEntryTable);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
