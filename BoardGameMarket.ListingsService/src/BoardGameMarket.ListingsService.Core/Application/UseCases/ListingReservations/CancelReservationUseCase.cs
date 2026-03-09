using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingReservations;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingsSold;
using BoardGameMarket.ListingsService.Core.Application.Contracts;
using BoardGameMarket.ListingsService.Core.Application.Exceptions;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingReservations
{
    public class CancelReservationUseCase
    {
        private readonly IListingReservationReadService _listingReservationReadService;
        private readonly IListingReservationWriteService _listingReservationWriteService;
        private readonly IListingSoldReadService _listingSoldReadService;
        private readonly IListingReadService _listingReadService;
        private readonly IListingWriteService _listingWriteService;
        private readonly IListingQueueEntryReadService _listingQueueEntryReadService;
        private readonly IListingQueueEntryWriteService _listingQueueEntryWriteService;
        private readonly IUnitOfWork _unitOfWork;

        public CancelReservationUseCase(IListingReservationReadService listingReservationReadService, IListingReservationWriteService listingReservationWriteService, IListingSoldReadService listingSoldReadService, IListingReadService listingReadService, IListingWriteService listingWriteService, IListingQueueEntryReadService listingQueueEntryReadService, IListingQueueEntryWriteService listingQueueEntryWriteService, IUnitOfWork unitOfWork)
        {
            _listingReservationReadService=listingReservationReadService;
            _listingReservationWriteService=listingReservationWriteService;
            _listingSoldReadService=listingSoldReadService;
            _listingReadService=listingReadService;
            _listingWriteService=listingWriteService;
            _listingQueueEntryReadService=listingQueueEntryReadService;
            _listingQueueEntryWriteService=listingQueueEntryWriteService;
            _unitOfWork=unitOfWork;
        }

        public async Task ExecuteAsync(Guid reservationId, Guid requesterUserId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                ListingReservation? reservation = await _listingReservationReadService.GetListingReservationByIdAsync(reservationId);
                if (reservation == null)
                {
                    throw new KeyNotFoundException($"Reservation {reservationId} not found");
                }
                if (reservation.SellerUserId != requesterUserId)
                {
                    throw new UnauthorizedAccessException();
                }
                ListingSold? existingListingSold = await _listingSoldReadService.GetListingSoldByListingId(reservation.ListingId);
                if (existingListingSold != null)
                {
                    throw new InvalidOperationException("Listing already sold");
                }
                if (reservation.Status != ReservationStatus.Reserved)
                {
                    throw new InvalidOperationException($"Reservation state: {reservation.Status}, expected {ReservationStatus.Reserved}");
                }
                Listing? listing = await _listingReadService.GetListingByIdAsync(reservation.ListingId);
                if (listing == null)
                {
                    throw new ListingNotFoundException(reservation.ListingId);
                }
                ListingQueueEntry? queueEntry = await _listingQueueEntryReadService.GetListingQueueEntryByIdAsync(reservation.ListingQueueEntryId);
                if (queueEntry == null)
                {
                    throw new KeyNotFoundException();
                }

                reservation.Status = ReservationStatus.Cancelled;
                listing.Status = ListingStatus.Active;
                queueEntry.Status = QueueStatus.Queued;
                await _listingReservationWriteService.UpdateListingReservationAsync(reservation);
                await _listingWriteService.UpdateListingAsync(listing);
                await _listingQueueEntryWriteService.UpdateListingQueueEntryAsync(queueEntry);

                await _unitOfWork.CommitTransactionAsync();
            }

            catch
            {
                throw;
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
    }
}
