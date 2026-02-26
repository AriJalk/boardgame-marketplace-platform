using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingReservations
{
    public interface IListingReservationReadService
    {
        Task<ListingReservation?> GetListingReservationByIdAsync(Guid listingReservationId);
        Task<ListingReservation?> GetListingReservationByListingId(Guid listingId);
        Task<IEnumerable<ListingReservation>> GetListingsReservationByBuyerUserIdAsync(Guid buyerUserId);
        Task<IEnumerable<ListingReservation>> GetListingsReservationBySellerUserIdAsync(Guid sellerUserId);
    }
}
