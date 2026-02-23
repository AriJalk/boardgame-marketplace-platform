import {
  CreateListingRequestDto as CreateListingRequest,
  ListingDetail,
  ListingSummary,
} from '../../listings/models/listing.model';
import { UserSummary } from '../../../core/users/models/user.model';

export interface ListingGroup {
  id: string;
  seller: UserSummary;
  postingTime: Date;
  pickupOptions: string;
  location: string;
  details?: string;
}

export interface ListingGroupSummary extends ListingGroup {
  listings: ListingSummary[];
}

export interface ListingGroupDetails extends ListingGroup {
  listings: ListingDetail[];
}

export interface CreateListingGroupRequest {
  details?: string;
  pickupOptions?: string;
  location: string;
  listings: CreateListingRequest[];
}
