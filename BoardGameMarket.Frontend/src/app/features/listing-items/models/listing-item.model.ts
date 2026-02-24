import { CatalogItem } from '../../catalog/models/catalog-item.model';

export interface ListingItem {
  id: string;
  catalogItem: CatalogItem;
  details?: string;
}

export interface CreateListingItemRequest {
  catalogId: string;
  details?: string;
}

export interface UpdateListingItemRequest {
  id?: string;
  catalogId: string;
  details?: string;
}
