import { Component, Input } from '@angular/core';
import { ListingDetail } from '../../../models/listing.model';
import { ListingItemCard } from '../../../../listing-items/components/display/listing-item-card/listing-item-card';

@Component({
  selector: 'bgm-listing-details-card',
  imports: [ListingItemCard],
  templateUrl: './listing-details-card.html',
  styleUrl: './listing-details-card.css',
})
export class ListingDetailsCard {
  @Input({ required: true }) listing!: ListingDetail | null;

}
