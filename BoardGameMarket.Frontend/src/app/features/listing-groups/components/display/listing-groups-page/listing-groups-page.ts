import { Component } from '@angular/core';
import { ListingGroupsGrid } from '../listing-groups-grid/listing-groups-grid';
import { PaginationControls } from "../../../../../common/pagination-controls/pagination-controls";

@Component({
  selector: 'bgm-listing-groups-page',
  imports: [ListingGroupsGrid, PaginationControls],
  templateUrl: './listing-groups-page.html',
  styleUrl: './listing-groups-page.css',
})
export class ListingGroupsPage {

  hasNext: boolean = true;
  hasPrevious: boolean = false;

  previous_clicked() {
    console.log("previous");
    this.hasPrevious = false;
    this.hasNext = true;

  }

  next_clicked() {
    console.log("next");
    this.hasNext = false;
    this.hasPrevious = true;
  }
}
