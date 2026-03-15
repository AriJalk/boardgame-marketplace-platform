using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingReservations;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Exceptions;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Queries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueue
{
    public class ReserveListingToQueueEntryUseCase
    {
        private readonly IListingQueueEntryReadService _listingQueueEntryReadService;
        private readonly IListingReadService _listingReadService;
        private readonly IListingWriteService _listingWriteService;
        private readonly IListingQueueEntryWriteService _listingQueueEntryWriteService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IListingReservationReadService _listingReservationReadService;
        private readonly IListingReservationWriteService _listingReservationWriteService;

        public ReserveListingToQueueEntryUseCase(IListingQueueEntryReadService listingQueueEntryReadService, IListingReadService listingReadService, IListingWriteService listingWriteService, IListingQueueEntryWriteService listingQueueEntryWriteService, IUnitOfWork unitOfWork, IListingReservationReadService listingReservationReadService, IListingReservationWriteService listingReservationWriteService)
        {
            _listingQueueEntryReadService = listingQueueEntryReadService;
            _listingReadService = listingReadService;
            _listingWriteService = listingWriteService;
            _listingQueueEntryWriteService = listingQueueEntryWriteService;
            _unitOfWork = unitOfWork;
            _listingReservationReadService = listingReservationReadService;
            _listingReservationWriteService = listingReservationWriteService;
        }

        public async Task<Guid> ExecuteAsync(Guid listingId, Guid listingQueueEntryId, Guid requesterUserId)
        {
            Listing? listing = await _listingReadService.GetListingByIdAsync(listingId);
            if (listing == null)
            {
                throw new ListingNotFoundException(listingId);
            }
            if (listing.Status != ListingStatus.Active)
            {
                throw new InvalidOperationException("Listing not in active status");
            }
            if (listing.SellerUserId != requesterUserId)
            {
                throw new UnauthorizedAccessException();
            }
            ListingReservation? existingReservation = await _listingReservationReadService.GetListingReservationByListingId(listingId);
            if (existingReservation != null && existingReservation.Status == ReservationStatus.Reserved)
            {
                return existingReservation.Id;
            }

            ListingQueueEntryFilter filter = new ListingQueueEntryFilter()
            {
                ListingIds = [listingId],
            };
            IEnumerable<ListingQueueEntry> queue = await _listingQueueEntryReadService.GetFilteredListingQueueEntriesAsync(filter);
            ListingQueueEntry requestedQueueEntry = queue.First(qe => qe.Id == listingQueueEntryId);
            QueueDomainService.ValidateReserve(queue, requestedQueueEntry);

            listing.Status = ListingStatus.Reserved;
            requestedQueueEntry.Status = QueueStatus.Reserved;
            ListingReservation reservationToAdd = new ListingReservation()
            {
                Id = Guid.NewGuid(),
                ListingId = listingId,
                ListingQueueEntryId = requestedQueueEntry.Id,
                BuyerUserId = requestedQueueEntry.BuyerUserId,
                SellerUserId = listing.SellerUserId,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                Status = ReservationStatus.Reserved,
            };

            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                await _listingWriteService.UpdateListingAsync(listing);
                await _listingQueueEntryWriteService.UpdateListingQueueEntryAsync(requestedQueueEntry);
                await _listingReservationWriteService.AddListingReservationAsync(reservationToAdd);
            });

            return reservationToAdd.Id;

        }
    }
}
