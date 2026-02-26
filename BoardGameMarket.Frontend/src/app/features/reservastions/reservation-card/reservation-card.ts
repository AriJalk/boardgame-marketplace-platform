import { Component, Input } from '@angular/core';
import { Reservation } from '../models/reservation.model';

@Component({
  selector: 'bgm-reservation-card',
  imports: [],
  templateUrl: './reservation-card.html',
  styleUrl: './reservation-card.css',
})
export class ReservationCard {
  @Input({ required: true }) reservation!: Reservation;
}
