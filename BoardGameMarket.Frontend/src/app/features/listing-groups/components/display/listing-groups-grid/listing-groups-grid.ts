import { Component } from '@angular/core';
import { MatGridListModule } from '@angular/material/grid-list';
import { ListingGroupCard } from "../listing-group-card/listing-group-card";

@Component({
  selector: 'bgm-listing-groups-grid',
  imports: [MatGridListModule, ListingGroupCard],
  templateUrl: './listing-groups-grid.html',
  styleUrl: './listing-groups-grid.css',
})
export class ListingGroupsGrid {

}
