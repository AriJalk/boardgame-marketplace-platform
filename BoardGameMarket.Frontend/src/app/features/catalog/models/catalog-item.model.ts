export interface CatalogItem {
  id: string;
  name: string;
  type: 'bgg' | 'custom';
  bggUrl: string | null;
  imageUrl: string | null;
}

export interface BggItem extends CatalogItem {}

export interface CustomItem extends CatalogItem {}
