import { Component, Input } from '@angular/core';
import { ListingSale } from '../../models/listing-sale.model';
import { ReservationCard } from '../../../reservations/reservation-card/reservation-card';

@Component({
  selector: 'bgm-listing-sale-card',
  imports: [ReservationCard],
  templateUrl: './listing-sale-card.html',
  styleUrl: './listing-sale-card.css',
})
export class ListingSaleCard {
  @Input() listingSale!: ListingSale;
}
