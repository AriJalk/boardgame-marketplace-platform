import {
  CreateListingItemRequest,
  ListingItem,
} from '../../listing-items/models/listing-item.model';
import { ListingSubscription, QueueEntry } from '../../queues/models/quque-entry.model';

export interface ListingSummary {
  id: string;
  title: string;
  price: number;
  status: string;
  details?: string;
  isSubscribed?: boolean;
}

export interface ListingDetail extends ListingSummary {
  items: ListingItem[];
  subscription?: ListingSubscription;
}

export interface CreateListingRequestDto {
  title: string;
  price: number;
  listingItems: CreateListingItemRequest[];
  details?: string;
}
