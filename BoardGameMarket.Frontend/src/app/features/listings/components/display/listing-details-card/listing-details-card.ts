import { Component, Input } from '@angular/core';
import { ListingDetail } from '../../../models/listing.model';
import { BggItem, CatalogItem, CustomItem } from '../../../../catalog/models/catalog-item.model';
import { BggItemCard } from '../../../../catalog/components/display/bgg-item-card/bgg-item-card';
import { CustomItemCard } from '../../../../catalog/components/display/custom-item-card/custom-item-card';
import { ListingItemCard } from '../../../../listing-items/components/listing-item-card/listing-item-card';

@Component({
  selector: 'bgm-listing-details-card',
  imports: [ListingItemCard],
  templateUrl: './listing-details-card.html',
  styleUrl: './listing-details-card.css',
})
export class ListingDetailsCard {
  @Input({ required: true }) listing!: ListingDetail;

}
