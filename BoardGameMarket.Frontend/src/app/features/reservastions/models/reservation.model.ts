export interface Reservation {
  id: string;
  listingId: string;
  listingQueueEntryId: string;
  reservedAt: Date;
  sellerId: string;
  buyerId: string;
}
