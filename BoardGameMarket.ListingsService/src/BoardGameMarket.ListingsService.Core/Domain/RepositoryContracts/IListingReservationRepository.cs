using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts
{
    public interface IListingReservationRepository
    {
        Task<ListingReservation?> GetListingReservationByIdAsync(Guid listingReservationId);
        Task<ListingReservation?> GetListingReservationByListingIdAsync(Guid listingId);
        Task<IEnumerable<ListingReservation>> GetListingsReservationsByBuyerUserIdAsync(Guid buyerUserId);
        Task<IEnumerable<ListingReservation>> GetListingsReservationsBySellerUserIdAsync(Guid sellerUserId);
        Task<int> AddListingReservationAsync(ListingReservation listingReservation);
        Task<int> DeleteListingReservationAsync(Guid listingReservationId);
        Task<int> UpdateListingReservationAsync(ListingReservation listingReservation);
    }
}
