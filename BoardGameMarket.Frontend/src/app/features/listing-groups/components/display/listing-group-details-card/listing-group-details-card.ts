import { ChangeDetectionStrategy, Component, Input, TemplateRef } from '@angular/core';
import { ListingGroupDetails } from '../../../models/listing-group.model';
import { MatCardModule } from '@angular/material/card';
import { ListingDetailsCard } from '../../../../listings/components/display/listing-details-card/listing-details-card';
import { NgTemplateOutlet } from '@angular/common';

@Component({
  selector: 'bgm-listing-group-details-card',
  imports: [MatCardModule, ListingDetailsCard, NgTemplateOutlet],
  templateUrl: './listing-group-details-card.html',
  styleUrl: './listing-group-details-card.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ListingGroupDetailsCard {
  @Input({ required: true }) listingGroup!: ListingGroupDetails;
  @Input() listingExtraTemplate!: TemplateRef<any>;
}
