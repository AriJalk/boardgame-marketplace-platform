import { Component } from '@angular/core';
import { ListingSummaryCard } from '../../../../listings/components/display/listing-summary-card/listing-summary-card';
import { ListingGroupSummary } from '../../../models/listing-group.model';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'bgm-listing-group-card',
  imports: [ListingSummaryCard, RouterLink],
  templateUrl: './listing-group-card.html',
  styleUrl: './listing-group-card.css',
})
export class ListingGroupCard {
  listingGroup: ListingGroupSummary = {
    id: "1",
    seller: { id: "1", displayName: "Test seller" },
    location: "BS",
    pickupOptions: "Pickup",
    details: "Arrange details",
    listings: [
      { id: "1", price: 30, title: "Puerto Rico Bundle" },
      { id: "2", price: 50, title: "Agricola Bundle" },
    ]

  };
}
