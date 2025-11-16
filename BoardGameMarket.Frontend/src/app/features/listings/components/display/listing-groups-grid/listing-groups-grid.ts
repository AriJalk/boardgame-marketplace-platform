import { Component } from '@angular/core';
import { MatGridListModule } from '@angular/material/grid-list';
import { ListingSummaryCard } from "../listing-summary-card/listing-summary-card";

@Component({
  selector: 'bgm-listing-groups-grid',
  imports: [MatGridListModule, ListingSummaryCard],
  templateUrl: './listing-groups-grid.html',
  styleUrl: './listing-groups-grid.css',
})
export class ListingGroupsGrid {

}
