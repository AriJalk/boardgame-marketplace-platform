import { Component, Input } from '@angular/core';
import { ListingSummaryCard } from '../../../../listings/components/display/listing-summary-card/listing-summary-card';
import { ListingGroupSummary } from '../../../models/listing-group.model';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'bgm-listing-group-summary-card',
  imports: [ListingSummaryCard, RouterLink, MatCardModule, MatButtonModule, DatePipe],
  templateUrl: './listing-group-summary-card.html',
  styleUrl: './listing-group-summary-card.css',
})
export class ListingGroupSummaryCard {
  @Input({ required: true }) listingGroup!: ListingGroupSummary;
}
