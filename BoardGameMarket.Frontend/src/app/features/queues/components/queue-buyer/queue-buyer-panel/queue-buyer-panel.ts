import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { QueueService } from '../../../queue.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { QueueEntry } from '../../../models/quque-entry.model';
import { AsyncPipe, DatePipe } from '@angular/common';

@Component({
  selector: 'bgm-queue-buyer-panel',
  imports: [MatButtonModule, DatePipe],
  templateUrl: './queue-buyer-panel.html',
  styleUrl: './queue-buyer-panel.css',
})
export class QueueBuyerPanel {
  @Input() queueEntry: QueueEntry | null | undefined;

  @Output() subscribeClicked = new EventEmitter();
  @Output() unsubscribeClicked = new EventEmitter();

  constructor(private queueService: QueueService) {}

  onSubscribed() {
    this.subscribeClicked.emit();
  }

  onUnsubscribed() {
    this.unsubscribeClicked.emit();
  }
}
