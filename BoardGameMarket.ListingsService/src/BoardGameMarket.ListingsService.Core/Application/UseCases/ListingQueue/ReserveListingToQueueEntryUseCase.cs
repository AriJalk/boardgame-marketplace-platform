using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
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
        private readonly ListingOwnershipQuery _listingOwnershipQuery;
        private readonly IListingQueueEntryWriteService _listingQueueEntryWriteService;
        private readonly IUnitOfWork _unitOfWork;

        public ReserveListingToQueueEntryUseCase(IListingQueueEntryReadService listingQueueEntryReadService, IListingReadService listingReadService, IListingWriteService listingWriteService, ListingOwnershipQuery listingOwnershipQuery, IListingQueueEntryWriteService listingQueueEntryWriteService, IUnitOfWork unitOfWork)
        {
            _listingQueueEntryReadService = listingQueueEntryReadService;
            _listingReadService = listingReadService;
            _listingWriteService = listingWriteService;
            _listingOwnershipQuery = listingOwnershipQuery;
            _listingQueueEntryWriteService = listingQueueEntryWriteService;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(Guid listingId, Guid listingQueueEntryId, Guid requesterUserId)
        {
            Guid sellerId = await _listingOwnershipQuery.GetSellerUserIdByListingIdAsync(listingId);
            if (sellerId != requesterUserId)
            {
                throw new UnauthorizedAccessException();
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                Listing? listing = await _listingReadService.GetListingByIdAsync(listingId);
                if (listing == null)
                {
                    throw new KeyNotFoundException();
                }
                if (listing.Status != ListingStatus.Active)
                {
                    throw new InvalidOperationException("Listing not in active status");
                }

                ListingQueueEntryFilter filter = new ListingQueueEntryFilter()
                {
                    ListingIds = [listingId],
                };
                IEnumerable<ListingQueueEntry> queue = await _listingQueueEntryReadService.GetFilteredListingQueueEntriesAsync(filter);
                ListingQueueEntry requestedQueueEntry = queue.First(qe => qe.Id == listingQueueEntryId);
                QueueDomainService.ValidateReserve(queue, requestedQueueEntry);

                listing.Status = ListingStatus.Reserved;
                requestedQueueEntry.Status = QueueStatus.Approved;

                await _listingWriteService.UpdateListingAsync(listing);
                await _listingQueueEntryWriteService.UpdateListingQueueEntryAsync(requestedQueueEntry);

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw;
            }
           
        }
    }
}
