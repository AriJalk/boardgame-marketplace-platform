import { Component } from '@angular/core';
import { MatGridListModule } from '@angular/material/grid-list';
import { ListingGroupSummaryCard } from "../listing-group-summary-card/listing-group-summary-card";
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'bgm-listing-groups-grid',
  imports: [MatGridListModule, ListingGroupSummaryCard, MatCardModule],
  templateUrl: './listing-groups-grid.html',
  styleUrl: './listing-groups-grid.css',
})
export class ListingGroupsGrid {

}
