import { Component, OnInit } from '@angular/core';
import { ReservationsService } from '../../reservations/services/reservations.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { Reservation } from '../../reservations/models/reservation.model';
import { AsyncPipe } from '@angular/common';
import { ReservationCard } from '../../reservations/reservation-card/reservation-card';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'bgm-my-reservations-page',
  imports: [AsyncPipe, ReservationCard],
  templateUrl: './my-reservations-page.html',
  styleUrl: './my-reservations-page.css',
})
export class MyReservationsPage implements OnInit {
  reservationsBehaviorSubject = new BehaviorSubject<Reservation[] | null>(null);
  reservations$ = this.reservationsBehaviorSubject.asObservable();

  constructor(
    private reservationsService: ReservationsService,
    private snackBar: MatSnackBar,
  ) {}

  ngOnInit(): void {
    this.reloadReservations();
  }

  reloadReservations() {
    this.reservationsService.getMyReservations().subscribe({
      next: (data) => {
        this.reservationsBehaviorSubject.next(data);
      },
    });
  }

  onSellRequested(reservation: Reservation) {
    this.reservationsService.sellReservation(reservation).subscribe({
      next: (data) => {
        this.snackBar.open('Sell successful', 'Close', { duration: 2000 });
        this.reloadReservations();
      },
    });
  }

  onCancelRequested(reservation: Reservation) {
    this.reservationsService.cancelReservation(reservation).subscribe({
      next: (data) => {
        this.snackBar.open('Cancel successful', 'Close', { duration: 2000 });
        this.reloadReservations();
      },
    });
  }
}
