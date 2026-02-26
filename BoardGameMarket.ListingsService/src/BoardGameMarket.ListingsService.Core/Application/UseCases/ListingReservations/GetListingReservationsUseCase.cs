using BoardGameMarket.ListingsService.Core.Application.Access.ListingReservations;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingReservations;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingReservations
{
    public class GetListingReservationsUseCase
    {
        private readonly IListingReservationReadService _listingReservationReadService;

        public GetListingReservationsUseCase(IListingReservationReadService listingReservationReadService)
        {
            _listingReservationReadService = listingReservationReadService;
        }

        public async Task<IReadOnlyCollection<ListingReservationInfo>> ExecuteAsync(Guid requesterUserId)
        {
            IEnumerable<ListingReservation> buyerReservations = await _listingReservationReadService.GetListingsReservationByBuyerUserIdAsync(requesterUserId);
            IEnumerable<ListingReservation> sellerReservations = await _listingReservationReadService.GetListingsReservationBySellerUserIdAsync(requesterUserId);

            IEnumerable<ListingReservationInfo> combined = buyerReservations.Concat(sellerReservations).OrderByDescending(lr => lr.ReservedAt).Select(lr => lr.ToListingReservationInfo());

            return combined.ToList();
        }
    }
}
