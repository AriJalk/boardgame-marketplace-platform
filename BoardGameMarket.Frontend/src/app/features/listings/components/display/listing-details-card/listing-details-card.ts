import { Component, Input } from '@angular/core';
import { ListingDetail } from '../../../models/listing.model';
import { BggItem, CatalogItem, CustomItem } from '../../../../catalog/models/catalog-item.model';
import { BggItemCard } from '../../../../catalog/components/display/bgg-item-card/bgg-item-card';
import { CustomItemCard } from '../../../../catalog/components/display/custom-item-card/custom-item-card';

@Component({
  selector: 'bgm-listing-details-card',
  imports: [BggItemCard, CustomItemCard],
  templateUrl: './listing-details-card.html',
  styleUrl: './listing-details-card.css',
})
export class ListingDetailsCard {
  @Input() listing?: ListingDetail;

  isBgg(item: CatalogItem): item is BggItem {
    return item.type === 'bgg';
  }
  isCustom(item: CatalogItem): item is CustomItem {
    return item.type === 'custom';
  }
}
