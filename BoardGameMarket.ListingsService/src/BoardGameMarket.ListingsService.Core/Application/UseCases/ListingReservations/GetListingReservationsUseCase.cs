using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingReservations;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingReservations;
using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Filters;
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
        private readonly IListingReadService _listingReadService;
        private readonly IListingQueueEntryReadService _listingQueueEntryReadService;

        public GetListingReservationsUseCase(IListingReservationReadService listingReservationReadService, IListingReadService listingReadService, IListingQueueEntryReadService listingQueueEntryReadService)
        {
            _listingReservationReadService = listingReservationReadService;
            _listingReadService = listingReadService;
            _listingQueueEntryReadService = listingQueueEntryReadService;
        }

        public async Task<IReadOnlyCollection<ListingReservationInfo>> ExecuteAsync(Guid requesterUserId)
        {
            IEnumerable<ListingReservation> buyerReservations = await _listingReservationReadService.GetListingsReservationByBuyerUserIdAsync(requesterUserId);
            IEnumerable<ListingReservation> sellerReservations = await _listingReservationReadService.GetListingsReservationBySellerUserIdAsync(requesterUserId);

            IEnumerable<ListingReservation> combined = buyerReservations.Concat(sellerReservations).OrderByDescending(lr => lr.ReservedAt);
            HashSet<Guid> listingIds = new HashSet<Guid>();
            HashSet<Guid> queueEntryIds = new HashSet<Guid>();
            foreach (ListingReservation reservation in combined)
            {
                listingIds.Add(reservation.ListingId);
                queueEntryIds.Add(reservation.ListingQueueEntryId);
            }
            ListingQueueEntryFilter listingQueueFilter = new ListingQueueEntryFilter()
            {
                ListingQueueEntryIds = queueEntryIds,
            };

            Task<IEnumerable<Listing>> listingsTask = _listingReadService.GetListingsByIdsAsync(listingIds);
            Task<IEnumerable<ListingQueueEntry>> queueEntriesTask = _listingQueueEntryReadService.GetFilteredListingQueueEntriesAsync(filter: listingQueueFilter);
            await Task.WhenAll(listingsTask, queueEntriesTask);

            Dictionary<Guid, Listing> listings = (await listingsTask).ToDictionary(l => l.Id);
            Dictionary<Guid, ListingQueueEntry> queueEntries = (await queueEntriesTask).ToDictionary(qe => qe.Id);

            List<ListingReservationInfo> dtos = new List<ListingReservationInfo>();
            foreach (ListingReservation reservation in combined)
            {
                ListingSummary listing = listings[reservation.ListingId].ToListingSummary(reservation.SellerUserId);
                ListingQueueEntryInfo queueEntry = queueEntries[reservation.ListingQueueEntryId].ToListingQueueEntryInfo();

                ListingReservationInfo reservationDto = reservation.ToListingReservationInfo(listing, queueEntry);
                dtos.Add(reservationDto);
            }

            return dtos;
        }
    }
}
