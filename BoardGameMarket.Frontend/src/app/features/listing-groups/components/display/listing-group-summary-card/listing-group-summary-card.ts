import { Component } from '@angular/core';
import { ListingSummaryCard } from '../../../../listings/components/display/listing-summary-card/listing-summary-card';
import { ListingGroupSummary } from '../../../models/listing-group.model';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'bgm-listing-group-summary-card',
  imports: [ListingSummaryCard, RouterLink, MatCardModule, MatButtonModule, DatePipe],
  templateUrl: './listing-group-summary-card.html',
  styleUrl: './listing-group-summary-card.css',
})
export class ListingGroupSummaryCard {
  listingGroup: ListingGroupSummary = {
    id: "127699fa-ff4c-4c12-b149-9534485a0129",
    seller: { id: "1", displayName: "Test seller" },
    postingTime: new Date(Date.now()),
    location: "BS",
    pickupOptions: "Pickup",
    details: "Arrange details",
    listings: [
      { id: "1", price: 30, title: "Puerto Rico Bundle", details: "complete" },
      { id: "2", price: 50, title: "Agricola Bundle", details: "complete" },
    ]

  };
}
