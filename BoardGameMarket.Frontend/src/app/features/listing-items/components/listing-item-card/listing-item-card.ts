import { Component, Input } from '@angular/core';
import { ListingItem } from '../../models/listing-item.model';
import { CatalogItemCard } from "../../../catalog/components/display/catalog-item-card/catalog-item-card";

@Component({
  selector: 'bgm-listing-item-card',
  imports: [CatalogItemCard],
  templateUrl: './listing-item-card.html',
  styleUrl: './listing-item-card.css',
})
export class ListingItemCard {
  @Input({ required: true }) listingItem!: ListingItem;

}
