export interface ListingGroupFilter {
  sellerUserId?: string;
  catalogId?: string;
  location?: string;
  listingGroupIds?: string[];
  listingIds?: string[];
}

export interface PagedListingGroupQuery {
  pageSize: number;
  cursor: string | null;
  filter?: ListingGroupFilter;
}
