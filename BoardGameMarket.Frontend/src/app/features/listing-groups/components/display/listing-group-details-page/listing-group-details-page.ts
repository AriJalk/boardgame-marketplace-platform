import { Component, OnInit } from '@angular/core';
import { ListingGroupDetails } from '../../../models/listing-group.model';
import { ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { ListingGroupService } from '../../../services/listing-group.service';
import { Observable } from 'rxjs';
import { ListingGroupDetailsCard } from '../listing-group-details-card/listing-group-details-card';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'bgm-listing-group-details-page',
  imports: [MatCardModule, ListingGroupDetailsCard, AsyncPipe],
  templateUrl: './listing-group-details-page.html',
  styleUrl: './listing-group-details-page.css',
})
export class ListingGroupDetailsPage implements OnInit {
  listingGroup$?: Observable<ListingGroupDetails>;

  constructor(
    private router: ActivatedRoute,
    private listingGroupService: ListingGroupService,
  ) {}

  ngOnInit(): void {
    this.router.paramMap.subscribe((params) => {
      const id = params.get('listingGroupId');
      if (id) {
        this.listingGroup$ = this.listingGroupService.fetchListingGroupDetails(id);
      }
    });
  }
}
