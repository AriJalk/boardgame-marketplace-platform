import { ChangeDetectionStrategy, Component, Input, TemplateRef } from '@angular/core';
import { ListingGroupDetails } from '../../../models/listing-group.model';
import { MatCardModule } from '@angular/material/card';
import { ListingDetailsCard } from '../../../../listings/components/display/listing-details-card/listing-details-card';
import { QueueBuyerPanel } from '../../../../queues/components/queue-buyer/queue-buyer-panel/queue-buyer-panel';
import { AuthStubService } from '../../../../../core/auth/auth-stub.service';

@Component({
  selector: 'bgm-listing-group-details-card',
  imports: [MatCardModule, ListingDetailsCard],
  templateUrl: './listing-group-details-card.html',
  styleUrl: './listing-group-details-card.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ListingGroupDetailsCard {
  @Input({ required: true }) listingGroup!: ListingGroupDetails;

  get isLoggedIn(): boolean {
    return this.authService.loggedInUser !== undefined;
  }

  constructor(private authService: AuthStubService) {}
}
