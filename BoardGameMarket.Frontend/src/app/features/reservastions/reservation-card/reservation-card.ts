import { Component, Input } from '@angular/core';
import { Reservation } from '../models/reservation.model';
import { ListingSummaryCard } from "../../listings/components/display/listing-summary-card/listing-summary-card";
import { RouterLink } from "@angular/router";

@Component({
  selector: 'bgm-reservation-card',
  imports: [ListingSummaryCard, RouterLink],
  templateUrl: './reservation-card.html',
  styleUrl: './reservation-card.css',
})
export class ReservationCard {
  @Input({ required: true }) reservation!: Reservation;
}
