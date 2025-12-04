import { Component, Input } from '@angular/core';
import { BggItem, CatalogItem, CustomItem } from '../../../models/catalog-item.model';
import { BggItemCard } from '../bgg-item-card/bgg-item-card';

@Component({
  selector: 'bgm-catalog-item-card',
  imports: [BggItemCard],
  templateUrl: './catalog-item-card.html',
  styleUrl: './catalog-item-card.css',
})

// Wrapper for catalog items
export class CatalogItemCard {
  @Input({ required: true }) catalogItem!: CatalogItem;

  isBgg(item: CatalogItem): item is BggItem {
    return this.catalogItem?.type === 'bgg';
  }

  isCustom(item: CatalogItem): item is CustomItem {
    return this.catalogItem?.type === 'custom';
  }
}