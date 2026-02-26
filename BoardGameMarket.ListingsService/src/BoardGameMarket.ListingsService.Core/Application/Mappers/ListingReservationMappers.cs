using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingReservations;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Mappers
{
    public static class ListingReservationMappers
    {
        public static ListingReservationInfo ToListingReservationInfo(this ListingReservation reservation)
        {
            return new ListingReservationInfo(reservation.Id, reservation.ListingId, reservation.ListingQueueEntryId, reservation.ReservedAt, reservation.SellerId, reservation.BuyerId);
        }
    }
}
