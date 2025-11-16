import { Component } from '@angular/core';
import { ListingSummary } from '../../../models/listing.model';
import { MatCardModule } from '@angular/material/card';
import { RouterLink, RouterModule } from "@angular/router";


@Component({
  selector: 'bgm-listing-card',
  imports: [MatCardModule, RouterLink, RouterModule],
  templateUrl: './listing-summary-card.html',
  styleUrl: './listing-summary-card.css',
})
export class ListingSummaryCard {

  listing: ListingSummary = {
    id: "1",
    title: "test listing",
    price: 20,
    sellerDisplayName: "Test seller"
  }
}
