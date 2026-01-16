export interface CatalogItem {
  id: string;
  name: string;
  type: 'bgg' | 'custom';
  bggUrl?: string;
  imageUrl?: string;
}

export interface BggItem extends CatalogItem {
  bggUrl: string;
  imageUrl?: string;
}

export interface CustomItem extends CatalogItem {}
