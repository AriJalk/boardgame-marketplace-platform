export interface CatalogItem {
    id: string;
    name: string;
    type: string;
}

export interface BggItem extends CatalogItem {
    bggLink: string;
    imageUrl?: string;
}

export interface CustomItem extends CatalogItem { }
