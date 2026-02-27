import { Component, Input } from '@angular/core';
import { Reservation } from '../models/reservation.model';
import { ListingSummaryCard } from '../../listings/components/display/listing-summary-card/listing-summary-card';
import { RouterLink } from '@angular/router';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'bgm-reservation-card',
  imports: [ListingSummaryCard, RouterLink, JsonPipe],
  templateUrl: './reservation-card.html',
  styleUrl: './reservation-card.css',
})
export class ReservationCard {
  @Input({ required: true }) reservation!: Reservation;
}
