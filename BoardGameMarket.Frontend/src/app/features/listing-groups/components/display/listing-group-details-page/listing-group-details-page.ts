import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ListingGroupDetails } from '../../../models/listing-group.model';
import { ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { ListingGroupService } from '../../../services/listing-group.service';
import { ListingGroupDetailsCard } from '../listing-group-details-card/listing-group-details-card';
import { QueueBuyerPanel } from '../../../../queues/components/queue-buyer/queue-buyer-panel/queue-buyer-panel';
import { AuthStubService } from '../../../../../core/auth/auth-stub.service';
import { Observable } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { UserSummary } from '../../../../../core/users/models/user.model';
import { QueueSellerPanel } from '../../../../queues/components/queue-seller/queue-seller-panel/queue-seller-panel';
import { EditListingGroup } from '../../edit-listing-group/edit-listing-group';

@Component({
  selector: 'bgm-listing-group-details-page',
  imports: [
    MatCardModule,
    ListingGroupDetailsCard,
    QueueBuyerPanel,
    AsyncPipe,
    QueueSellerPanel,
    EditListingGroup,
  ],
  templateUrl: './listing-group-details-page.html',
  styleUrl: './listing-group-details-page.css',
})
export class ListingGroupDetailsPage implements OnInit {
  listingGroup$?: Observable<ListingGroupDetails>;
  user?: UserSummary;

  constructor(
    private router: ActivatedRoute,
    private listingGroupService: ListingGroupService,
    private authService: AuthStubService,
  ) {}

  ngOnInit(): void {
    this.user = this.authService.loggedInUser;
    this.router.paramMap.subscribe((params) => {
      const id = params.get('listingGroupId');
      if (id) {
        this.listingGroup$ = this.listingGroupService.getListingGroupDetails(id);
      }
    });
  }
}
