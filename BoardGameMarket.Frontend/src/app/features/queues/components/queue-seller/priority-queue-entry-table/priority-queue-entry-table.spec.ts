import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PriorityQueueEntryTable } from './priority-queue-entry-table';

describe('PriorityQueueEntryTable', () => {
  let component: PriorityQueueEntryTable;
  let fixture: ComponentFixture<PriorityQueueEntryTable>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PriorityQueueEntryTable]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PriorityQueueEntryTable);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
