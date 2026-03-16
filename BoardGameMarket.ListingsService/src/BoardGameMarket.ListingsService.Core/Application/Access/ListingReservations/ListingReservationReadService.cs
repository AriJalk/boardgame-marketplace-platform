using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingReservations
{
    internal class ListingReservationReadService : IListingReservationReadService
    {
        private readonly IListingReservationRepository _listingReservationRepository;

        public ListingReservationReadService(IListingReservationRepository listingReservationRepository)
        {
            _listingReservationRepository = listingReservationRepository;
        }

        public async Task<ListingReservation?> GetListingReservationByIdAsync(Guid listingReservationId)
        {
            return await _listingReservationRepository.GetListingReservationByIdAsync(listingReservationId);
        }

        public async Task<ListingReservation?> GetListingReservationByListingId(Guid listingId)
        {
            return await _listingReservationRepository.GetListingReservationByListingIdAsync(listingId);
        }

        public async Task<IEnumerable<ListingReservation>> GetListingsReservationByBuyerUserIdAsync(Guid buyerUserId)
        {
            return await _listingReservationRepository.GetListingsReservationsByBuyerUserIdAsync(buyerUserId);
        }

        public async Task<IEnumerable<ListingReservation>> GetListingsReservationBySellerUserIdAsync(Guid sellerUserId)
        {
            return await _listingReservationRepository.GetListingsReservationsBySellerUserIdAsync (sellerUserId);
        }
    }
}
