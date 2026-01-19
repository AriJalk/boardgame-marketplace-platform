import { Component, Input, OnInit } from '@angular/core';
import { QueueEntry } from '../../../models/quque-entry.model';
import { MatCardModule } from '@angular/material/card';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'bgm-queue-entry-card',
  imports: [MatCardModule, DatePipe],
  templateUrl: './queue-entry-card.html',
  styleUrl: './queue-entry-card.css',
})
export class QueueEntryCard {
  @Input({ required: true }) queueEntry!: QueueEntry;
}
