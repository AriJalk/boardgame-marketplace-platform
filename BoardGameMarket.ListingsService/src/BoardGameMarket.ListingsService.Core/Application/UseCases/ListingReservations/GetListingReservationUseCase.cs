using BoardGameMarket.ListingsService.Core.Application.Access.ListingReservations;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingReservations;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingReservations
{
    public class GetListingReservationUseCase
    {
        private readonly IListingReservationReadService _listingReservationReadService;

        public GetListingReservationUseCase(IListingReservationReadService listingReservationReadService)
        {
            _listingReservationReadService = listingReservationReadService;
        }

        public async Task<ListingReservationInfo> ExecuteAsync(Guid listingReservationId, Guid requesterUserId)
        {
            ListingReservation? listingReservation = await _listingReservationReadService.GetListingReservationByIdAsync(listingReservationId);
            if (listingReservation == null)
            {
                throw new KeyNotFoundException($"Reservation {listingReservationId}");
            }
            if (listingReservation.SellerId != requesterUserId && listingReservation.BuyerId != requesterUserId)
            {
                throw new UnauthorizedAccessException();
            }
            return listingReservation.ToListingReservationInfo();
        }
    }
}
