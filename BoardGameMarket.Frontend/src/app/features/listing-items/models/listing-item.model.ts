import { CatalogItem } from "../../catalog/models/catalog-item.model";

export interface ListingItem {
    id: string;
    catalogItem: CatalogItem;
    details?: string;
}