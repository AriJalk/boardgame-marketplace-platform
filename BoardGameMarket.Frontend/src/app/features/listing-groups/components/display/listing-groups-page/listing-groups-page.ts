import { Component, OnInit } from '@angular/core';
import { PaginationControls } from '../../../../../common/pagination-controls/pagination-controls';
import { ListingGroupService } from '../../../services/listing-group.service';
import { ListingGroupSummary } from '../../../models/listing-group.model';
import { AsyncPipe, CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { ListingGroupSummaryCard } from '../listing-group-summary-card/listing-group-summary-card';

@Component({
  selector: 'bgm-listing-groups-page',
  imports: [PaginationControls, CommonModule, AsyncPipe, ListingGroupSummaryCard],
  templateUrl: './listing-groups-page.html',
  styleUrl: './listing-groups-page.css',
})
export class ListingGroupsPage implements OnInit {
  hasNext: boolean = true;
  hasPrevious: boolean = false;

  listingGroups$!: Observable<ListingGroupSummary[]>;

  constructor(private listingGroupService: ListingGroupService) {}

  ngOnInit(): void {
    this.listingGroups$ = this.listingGroupService.getListingGroupSummaries();
  }

  previous_clicked() {
    console.log('previous');
    this.hasPrevious = false;
    this.hasNext = true;
  }

  next_clicked() {
    console.log('next');
    this.hasNext = false;
    this.hasPrevious = true;
  }
}
