import { Component, Input, OnInit } from '@angular/core';
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
  @Input({ required: true }) queueEntry!: QueueEntry | null;

  constructor(private queueService: QueueService) {}

  onSubscribed() {}

  onUnsubscribed(subscriptionId: string) {
    
  }
}
