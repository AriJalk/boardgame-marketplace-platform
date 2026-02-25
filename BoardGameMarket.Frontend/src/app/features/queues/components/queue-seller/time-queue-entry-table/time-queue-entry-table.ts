import { ScrollingModule } from '@angular/cdk/scrolling';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { QueueEntry } from '../../../models/quque-entry.model';
import { DatePipe } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'bgm-time-queue-entry-table',
  imports: [MatTableModule, ScrollingModule, DatePipe, MatIconModule, MatButtonModule],
  templateUrl: './time-queue-entry-table.html',
  styleUrl: './time-queue-entry-table.css',
})
export class TimeQueueEntryTable {
  @Input({ required: true }) queue!: QueueEntry[];
  @Input() isLocked: boolean = false;

  @Output() priorityRequested = new EventEmitter<QueueEntry>();
  @Output() deleteRequested = new EventEmitter<QueueEntry>();

  displayedColumns: string[] = ['createdAt', 'buyer', 'status', 'priority', 'options'];
  trackBy = (index: number, el: QueueEntry) => el.id;

  onPriorityClicked(queueEntry: QueueEntry) {
    this.priorityRequested.emit(queueEntry);
  }

  onDeleteClicked(queueEntry: QueueEntry) {
    this.deleteRequested.emit(queueEntry);
  }
}
