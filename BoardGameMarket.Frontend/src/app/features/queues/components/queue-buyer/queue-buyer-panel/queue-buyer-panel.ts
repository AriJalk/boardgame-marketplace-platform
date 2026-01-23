import { Component, Input, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { QueueService } from '../../../queue.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { QueueEntry } from '../../../models/quque-entry.model';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'bgm-queue-buyer-panel',
  imports: [MatButtonModule, AsyncPipe],
  templateUrl: './queue-buyer-panel.html',
  styleUrl: './queue-buyer-panel.css',
})
export class QueueBuyerPanel implements OnInit {
  @Input({ required: true }) listingId!: string;

  private _queueEntry = new BehaviorSubject<QueueEntry | undefined>(undefined);
  queueEntry$ = this._queueEntry.asObservable();

  constructor(private queueService: QueueService) {}

  ngOnInit(): void {
    this.queueService.getSubscription(this.listingId).subscribe({
      next: (data) => {
        this._queueEntry.next(data);
      },
    });
  }

  onSubscribed() {
    this.queueService.subscribe(this.listingId).subscribe({
      next: (data) => {
        this._queueEntry.next(data);
      },
    });
  }

  onUnsubscribed(subscriptionId: string) {
    this.queueService.unsubscribe(this.listingId, subscriptionId).subscribe({
      next: () => {
        this._queueEntry.next(undefined);
      },
    });
  }
}
