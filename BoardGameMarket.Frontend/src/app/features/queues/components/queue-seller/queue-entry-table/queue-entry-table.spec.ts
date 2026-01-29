import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QueueEntryTable } from './queue-entry-table';

describe('QueueEntryTable', () => {
  let component: QueueEntryTable;
  let fixture: ComponentFixture<QueueEntryTable>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QueueEntryTable]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QueueEntryTable);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
