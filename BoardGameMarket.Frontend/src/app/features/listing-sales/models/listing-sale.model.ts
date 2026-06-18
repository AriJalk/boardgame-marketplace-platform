import { Reservation } from '../../reservations/models/reservation.model';

export interface ListingSale {
  id: string;
  completedAt: Date;
  reservation: Reservation;
}
