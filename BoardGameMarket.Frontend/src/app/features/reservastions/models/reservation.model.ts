import { ListingGroupMetadata } from '../../listing-groups/models/listing-group.model';
import { ListingSummary } from '../../listings/models/listing.model';
import { QueueEntry } from '../../queues/models/quque-entry.model';

export interface Reservation {
  id: string;
  listing: ListingSummary;
  listingQueueEntry: QueueEntry;
  reservedAt: Date;
  listingGroupMetadata: ListingGroupMetadata;
  status: string;
}
