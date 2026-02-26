import { Component, OnInit } from '@angular/core';
import { ReservationsService } from '../../reservastions/services/reservations.service';
import { Observable } from 'rxjs';
import { Reservation } from '../../reservastions/models/reservation.model';
import { AsyncPipe } from '@angular/common';
import { ReservationCard } from '../../reservastions/reservation-card/reservation-card';

@Component({
  selector: 'bgm-my-reservations-page',
  imports: [AsyncPipe, ReservationCard],
  templateUrl: './my-reservations-page.html',
  styleUrl: './my-reservations-page.css',
})
export class MyReservationsPage implements OnInit {
  reservations$!: Observable<Reservation[]>;

  constructor(private reservationsService: ReservationsService) {}

  ngOnInit(): void {
    this.reservations$ = this.reservationsService.getMyReservations();
    this.reservations$.subscribe((data) => console.log(data));
  }
}
