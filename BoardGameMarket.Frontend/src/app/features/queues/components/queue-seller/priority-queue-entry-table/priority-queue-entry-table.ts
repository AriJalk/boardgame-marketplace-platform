import { CdkDrag, CdkDragDrop, CdkDropList, moveItemInArray } from '@angular/cdk/drag-drop';
import { DatePipe } from '@angular/common';
import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatTable, MatTableModule } from '@angular/material/table';
import { QueueEntry, UpdatePriorityRequest } from '../../../models/quque-entry.model';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'bgm-priority-queue-entry-table',
  imports: [CdkDropList, CdkDrag, MatTableModule, MatIconModule, DatePipe, MatButtonModule],
  templateUrl: './priority-queue-entry-table.html',
  styleUrl: './priority-queue-entry-table.css',
})
export class PriorityQueueEntryTable {
  @Input({ required: true }) queue!: QueueEntry[];

  @Output() deletePriorityRequested = new EventEmitter<QueueEntry>();
  @Output() updatePriorityRequested = new EventEmitter<UpdatePriorityRequest>();

  @ViewChild('table', { static: true }) table!: MatTable<QueueEntry>;
  displayedColumns: string[] = ['position', 'createdAt', 'buyer', 'status', 'priority', 'options'];

  trackBy = (index: number, el: QueueEntry) => el.id;

  drop(event: CdkDragDrop<QueueEntry[]>) {
    // const previousIndex = this.queue.findIndex((d) => d === event.item.data);

    const queueEntry = event.item.data as QueueEntry;
    this.updatePriorityRequested.emit({
      queueEntry: queueEntry,
      requestedIndex: event.currentIndex,
    });

    // moveItemInArray(this.queue, previousIndex, event.currentIndex);
    // this.table.renderRows();
  }

  onDeleteClicked(queueEntry: QueueEntry) {
    this.deletePriorityRequested.emit(queueEntry);
  }
}
