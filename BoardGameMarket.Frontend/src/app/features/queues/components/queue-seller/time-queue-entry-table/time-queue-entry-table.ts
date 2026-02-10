import { ScrollingModule } from '@angular/cdk/scrolling';
import { Component, Input, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { QueueEntry } from '../../../models/quque-entry.model';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'bgm-time-queue-entry-table',
  imports: [MatTableModule, ScrollingModule, DatePipe],
  templateUrl: './time-queue-entry-table.html',
  styleUrl: './time-queue-entry-table.css',
})
export class TimeQueueEntryTable implements OnInit {
  @Input({ required: true }) queue!: QueueEntry[];
  displayedColumns: string[] = ['createdAt', 'buyer', 'status', 'priority'];
  trackBy = (index: number, el: QueueEntry) => el.id;

  ngOnInit(): void {}
}
