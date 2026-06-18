import { UserSummary } from '../../../core/users/models/user.model';
import {
  CreateListingItemRequest,
  ListingItem,
  UpdateListingItemRequest,
} from '../../listing-items/models/listing-item.model';
import { ListingSubscription, QueueEntry } from '../../queues/models/queue-entry.model';

export interface ListingSummary {
  id: string;
  title: string;
  price: number;
  status: string;
  details?: string;
  isSubscribed?: boolean;
  seller: UserSummary;
}

export interface ListingDetail extends ListingSummary {
  items: ListingItem[];
  subscription?: ListingSubscription;
}

export interface CreateListingRequest {
  title: string;
  price: number;
  listingItems: CreateListingItemRequest[];
  details?: string;
}

export interface UpdateListingRequest {
  id?: string;
  title: string;
  price: number;
  listingItems: UpdateListingItemRequest[];
  details?: string;
}
