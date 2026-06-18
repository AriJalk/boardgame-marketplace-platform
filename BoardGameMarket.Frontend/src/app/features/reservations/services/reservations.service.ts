import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Reservation } from '../models/reservation.model';
import { API_CONFIG_SERVICE } from '../../../core/api-config/api-config.service';
import { ApiConfig } from '../../../core/api-config/api-config.interface';

@Injectable({
  providedIn: 'root',
})
export class ReservationsService {
  constructor(
    private httpClient: HttpClient,
    @Inject(API_CONFIG_SERVICE) private apiConfig: ApiConfig,
  ) {}

  getMyReservations(): Observable<Reservation[]> {
    return this.httpClient.get<Reservation[]>(`${this.apiConfig.apiEndpoint}/reservations/my`);
  }

  sellReservation(reservation: Reservation) {
    return this.httpClient.post<string>(
      `${this.apiConfig.apiEndpoint}/reservations/${reservation.id}/sell`,
      null,
    );
  }

  cancelReservation(reservation: Reservation) {
    return this.httpClient.post<string>(
      `${this.apiConfig.apiEndpoint}/reservations/${reservation.id}/cancel`,
      null,
    );
  }
}
