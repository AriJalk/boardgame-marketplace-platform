import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Reservation } from '../models/reservation.model';
import { ListingSummaryCard } from '../../listings/components/display/listing-summary-card/listing-summary-card';
import { Router, RouterLink } from '@angular/router';
import { JsonPipe } from '@angular/common';
import { MatAnchor, MatButtonModule } from '@angular/material/button';
import { ReservationsService } from '../services/reservations.service';

@Component({
  selector: 'bgm-reservation-card',
  imports: [ListingSummaryCard, RouterLink, MatAnchor, MatButtonModule],
  templateUrl: './reservation-card.html',
  styleUrl: './reservation-card.css',
})
export class ReservationCard {
  @Input({ required: true }) reservation!: Reservation;

  @Output() sellRequested = new EventEmitter<Reservation>();
  @Output() cancelRequested = new EventEmitter<Reservation>();

  requestSell() {
    this.sellRequested.emit(this.reservation);
  }

  requestCancel() {
    this.cancelRequested.emit(this.reservation);
  }
}
