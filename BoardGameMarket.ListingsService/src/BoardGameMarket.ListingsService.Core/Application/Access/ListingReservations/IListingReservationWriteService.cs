using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingReservations
{
    public interface IListingReservationWriteService
    {
        Task AddListingReservationAsync(ListingReservation listingReservationToAdd);
        Task DeleteListingReservationAsync(Guid listingReservationId);
    }
}
