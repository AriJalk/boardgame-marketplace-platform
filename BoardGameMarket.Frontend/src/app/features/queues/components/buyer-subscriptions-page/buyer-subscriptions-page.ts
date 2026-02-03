import { Component, OnInit } from '@angular/core';
import { ListingGroupService } from '../../../listing-groups/services/listing-group.service';
import { AuthStubService } from '../../../../core/auth/auth-stub.service';
import { Observable } from 'rxjs';
import { ListingGroupSummaryCard } from '../../../listing-groups/components/display/listing-group-summary-card/listing-group-summary-card';
import { ListingGroupSummary } from '../../../listing-groups/models/listing-group.model';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'bgm-buyer-subscriptions-page',
  imports: [AsyncPipe, ListingGroupSummaryCard],
  templateUrl: './buyer-subscriptions-page.html',
  styleUrl: './buyer-subscriptions-page.css',
})
export class BuyerSubscriptionsPage implements OnInit {
  listingGroups$!: Observable<ListingGroupSummary[]>;

  constructor(
    private listingGroupService: ListingGroupService,
    private authService: AuthStubService,
  ) {}

  ngOnInit(): void {
    this.listingGroups$ = this.listingGroupService.getSubscribedListingGroups();
  }
}
