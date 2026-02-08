import { ListingItem } from '../../listing-items/models/listing-item.model';
import { QueueEntry } from '../../queues/models/quque-entry.model';

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
  queueEntry?: QueueEntry;
}
