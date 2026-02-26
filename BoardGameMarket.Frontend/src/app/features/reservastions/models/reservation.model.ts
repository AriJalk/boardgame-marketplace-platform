import { ListingSummary } from '../../listings/models/listing.model';
import { QueueEntry } from '../../queues/models/quque-entry.model';

export interface Reservation {
  id: string;
  listing: ListingSummary;
  listingQueueEntry: QueueEntry;
  reservedAt: Date;
}
