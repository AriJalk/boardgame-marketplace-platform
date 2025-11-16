import { CatalogItem } from "../../catalog/models/catalog-item.model";

export interface ListingItem {
    catalogItem: CatalogItem;
    details?: string;
}