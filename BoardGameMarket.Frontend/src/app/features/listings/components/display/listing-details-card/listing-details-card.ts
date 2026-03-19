import { Component, Input, OnInit } from '@angular/core';
import { ListingDetail } from '../../../models/listing.model';
import { ListingItemCard } from '../../../../listing-items/components/display/listing-item-card/listing-item-card';
import { MatCardModule } from '@angular/material/card';
import { QueueBuyerPanel } from '../../../../queues/components/queue-buyer/queue-buyer-panel/queue-buyer-panel';
import { QueueService } from '../../../../queues/queue.service';
import { ListingSubscription, QueueEntry } from '../../../../queues/models/quque-entry.model';
import { BehaviorSubject, Observable, of, switchMap, take } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { AuthService } from '../../../../../core/auth/auth.service';
import { AccountInfo } from '@azure/msal-browser';

@Component({
  selector: 'bgm-listing-details-card',
  imports: [ListingItemCard, MatCardModule, QueueBuyerPanel, AsyncPipe],
  templateUrl: './listing-details-card.html',
  styleUrl: './listing-details-card.css',
})
export class ListingDetailsCard implements OnInit {
  private listingSubscriptionSubject = new BehaviorSubject<ListingSubscription | undefined>(
    undefined,
  );

  @Input({ required: true }) set listing(value: ListingDetail) {
    this._listing = value;
    this.listingSubscriptionSubject.next(value.subscription);
  }
  get listing() {
    return this._listing;
  }

  private _listing!: ListingDetail;

  listingSubscription$?: Observable<ListingSubscription | undefined> =
    this.listingSubscriptionSubject.asObservable();

  account$!: Observable<AccountInfo | null>;

  constructor(
    private queueService: QueueService,
    private authService: AuthService,
  ) {}

  ngOnInit(): void {
    this.account$ = this.authService.activeAccount$;
  }

  onSubscribeClicked() {
    this.authService
      .withAuthCall((header) => this.queueService.subscribeToListing(this.listing.id, header))
      .subscribe({
        next: (data) => {
          this.listingSubscriptionSubject.next(data);
        },
        error: (err) => {
          console.log('Subscription failed', err);
        },
      });
  }

  onUnsubscribeClicked() {
    const current = this.listingSubscriptionSubject.getValue();
    if (!current) {
      return;
    }

    this.authService
      .withAuthCall((header) =>
        this.queueService.unsubscribeFromListing(this.listing.id, current.id, header),
      )
      .pipe(take(1))
      .subscribe({
        next: () => {
          this.listingSubscriptionSubject.next(undefined);
        },
      });
  }
}
