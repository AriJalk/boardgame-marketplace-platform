import { Reservation } from '../../reservastions/models/reservation.model';

export interface ListingSale {
  id: string;
  completedAt: Date;
  reservation: Reservation;
}
