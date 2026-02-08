import { Component, Input, OnInit } from '@angular/core';
import { ListingDetail } from '../../../models/listing.model';
import { ListingItemCard } from '../../../../listing-items/components/display/listing-item-card/listing-item-card';
import { MatCardModule } from '@angular/material/card';
import { QueueBuyerPanel } from '../../../../queues/components/queue-buyer/queue-buyer-panel/queue-buyer-panel';
import { QueueService } from '../../../../queues/queue.service';
import { QueueEntry } from '../../../../queues/models/quque-entry.model';
import { BehaviorSubject, Observable, of, take } from 'rxjs';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'bgm-listing-details-card',
  imports: [ListingItemCard, MatCardModule, QueueBuyerPanel, AsyncPipe],
  templateUrl: './listing-details-card.html',
  styleUrl: './listing-details-card.css',
})
export class ListingDetailsCard {
  private queueEntrySubject = new BehaviorSubject<QueueEntry | undefined>(undefined);

  @Input({ required: true }) set listing(value: ListingDetail) {
    this._listing = value;
    this.queueEntrySubject.next(value.queueEntry);
  }
  get listing() {
    return this._listing;
  }

  private _listing!: ListingDetail;

  queueEntry$?: Observable<QueueEntry | undefined> = this.queueEntrySubject.asObservable();

  constructor(private queueService: QueueService) {}

  onSubscribeClicked() {
    this.queueService
      .subscribeToListing(this.listing.id)
      .pipe(take(1))
      .subscribe({
        next: (data) => {
          this.queueEntrySubject.next(data);
        },
      });
  }

  onUnsubscribeClicked() {
    const current = this.queueEntrySubject.getValue();
    if (!current) {
      return;
    }
    this.queueService
      .unsubscribeFromListing(this.listing.id, current.id)
      .pipe(take(1))
      .subscribe({
        next: () => {
          this.queueEntrySubject.next(undefined);
        },
      });
  }
}
