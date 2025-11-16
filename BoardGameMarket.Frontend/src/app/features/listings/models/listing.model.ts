import { CatalogItem } from "../../catalog/models/catalog-item.model";

export interface ListingSummary {
    id: string
    title: string
    sellerDisplayName: string
    price: number
}

export interface ListingDetail extends ListingSummary {
    items: CatalogItem[]
}

