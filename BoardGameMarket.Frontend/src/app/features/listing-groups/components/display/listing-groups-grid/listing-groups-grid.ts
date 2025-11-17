import { Component } from '@angular/core';
import { MatGridListModule } from '@angular/material/grid-list';
import { ListingGroupSummaryCard } from "../listing-group-summary-card/listing-group-summary-card";

@Component({
  selector: 'bgm-listing-groups-grid',
  imports: [MatGridListModule, ListingGroupSummaryCard],
  templateUrl: './listing-groups-grid.html',
  styleUrl: './listing-groups-grid.css',
})
export class ListingGroupsGrid {

}
