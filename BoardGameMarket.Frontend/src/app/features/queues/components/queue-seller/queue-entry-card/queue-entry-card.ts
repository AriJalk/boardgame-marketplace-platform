import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { QueueEntry } from '../../../models/quque-entry.model';
import { MatCardModule } from '@angular/material/card';
import { DatePipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'bgm-queue-entry-card',
  imports: [MatCardModule, DatePipe, MatButtonModule],
  templateUrl: './queue-entry-card.html',
  styleUrl: './queue-entry-card.css',
})
export class QueueEntryCard {
  @Input({ required: true }) queueEntry!: QueueEntry;
  @Output() removePriority = new EventEmitter<QueueEntry>();

  onRemovePriority(): void {
    this.removePriority.emit(this.queueEntry);
  }
}
