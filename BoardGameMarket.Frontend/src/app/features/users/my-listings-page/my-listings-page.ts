import { Component, OnInit } from '@angular/core';
import { elementAt, Observable, of, switchMap } from 'rxjs';
import { ListingGroupSummary } from '../../listing-groups/models/listing-group.model';
import { ListingGroupService } from '../../listing-groups/services/listing-group.service';
import { ListingSummaryCard } from '../../listings/components/display/listing-summary-card/listing-summary-card';
import { AsyncPipe } from '@angular/common';
import { MatCard, MatCardModule } from '@angular/material/card';
import { RouterLink } from '@angular/router';
import { MatAnchor, MatButtonModule } from '@angular/material/button';
import { ListingGroupSummaryCard } from '../../listing-groups/components/display/listing-group-summary-card/listing-group-summary-card';

@Component({
  selector: 'bgm-my-listings-page',
  imports: [
    AsyncPipe,
    ListingSummaryCard,
    MatCardModule,
    RouterLink,
    MatAnchor,
    MatButtonModule,
    ListingGroupSummaryCard,
  ],
  templateUrl: './my-listings-page.html',
  styleUrl: './my-listings-page.css',
})
export class MyListingsPage implements OnInit {
  myListingGroups$!: Observable<ListingGroupSummary[]>;
  mySubscribedListingGroups$!: Observable<ListingGroupSummary[]>;

  constructor(private listingGroupService: ListingGroupService) {}

  ngOnInit(): void {
    this.myListingGroups$ = this.listingGroupService.getMyListingGroups();
    this.mySubscribedListingGroups$ = this.listingGroupService.getMySubscribedListingGroups();
  }
}
