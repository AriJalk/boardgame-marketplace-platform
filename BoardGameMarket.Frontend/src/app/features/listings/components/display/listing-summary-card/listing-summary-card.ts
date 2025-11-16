import { Component, Input } from '@angular/core';
import { ListingSummary } from '../../../models/listing.model';
import { MatCardModule } from '@angular/material/card';
import { RouterLink, RouterModule } from "@angular/router";


@Component({
  selector: 'bgm-listing-card',
  imports: [MatCardModule, RouterModule],
  templateUrl: './listing-summary-card.html',
  styleUrl: './listing-summary-card.css',
})
export class ListingSummaryCard {

  @Input() listing!: ListingSummary;
}
