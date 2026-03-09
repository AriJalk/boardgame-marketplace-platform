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
    public class ApproveReservationSellUseCase
    {
        private readonly IListingReservationReadService _listingReservationReadService;
        private readonly IListingReservationWriteService _listingReservationWriteService;
        private readonly IListingSoldReadService _listingSoldReadService;
        private readonly IListingSoldWriteService _listingSoldWriteService;
        private readonly IListingReadService _listingReadService;
        private readonly IListingWriteService _listingWriteService;
        private readonly IUnitOfWork _unitOfWork;

        public ApproveReservationSellUseCase(IListingReservationReadService listingReservationReadService, IListingReservationWriteService listingReservationWriteService, IListingSoldReadService listingSoldReadService, IListingSoldWriteService listingSoldWriteService, IListingReadService listingReadService, IListingWriteService listingWriteService, IUnitOfWork unitOfWork)
        {
            _listingReservationReadService=listingReservationReadService;
            _listingReservationWriteService=listingReservationWriteService;
            _listingSoldReadService=listingSoldReadService;
            _listingSoldWriteService=listingSoldWriteService;
            _listingReadService=listingReadService;
            _listingWriteService=listingWriteService;
            _unitOfWork=unitOfWork;
        }

        public async Task<ListingSoldInfo> ExecuteAsync(Guid reservationId, Guid requesterUserId)
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
                    return existingListingSold.ToListingSoldInfo();
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
                ListingSold listingSold = new ListingSold()
                {
                    Id = Guid.NewGuid(),
                    ReservationId = reservationId,
                    CompletedAt = DateTime.UtcNow,
                    ListingQueueEntryId = reservation.ListingQueueEntryId,
                    BuyerUserId = reservation.BuyerUserId,
                    SellerUserId = reservation.SellerUserId,
                    ListingId = reservation.ListingId,
                };
                reservation.Status = ReservationStatus.Sold;
                listing.Status = ListingStatus.Sold;
                await _listingReservationWriteService.UpdateListingReservationAsync(reservation);
                await _listingSoldWriteService.AddListingSoldAsync(listingSold);
                await _listingWriteService.UpdateListingAsync(listing);

                await _unitOfWork.CommitTransactionAsync();

                return listingSold.ToListingSoldInfo();
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
