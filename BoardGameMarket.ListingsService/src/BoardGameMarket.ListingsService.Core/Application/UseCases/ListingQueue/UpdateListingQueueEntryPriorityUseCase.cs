using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Exceptions;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.Queries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueue
{
    public class UpdateListingQueueEntryPriorityUseCase
    {
        private readonly IListingQueueEntryReadService _listingQueueEntryReadService;
        private readonly IListingQueueEntryWriteService _listingQueueEntryWriteService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IListingReadService _listingReadService;

        public UpdateListingQueueEntryPriorityUseCase(IListingQueueEntryReadService listingQueueEntryReadService, IListingQueueEntryWriteService listingQueueEntryWriteService, IUnitOfWork unitOfWork, IListingReadService listingReadService)
        {
            _listingQueueEntryReadService = listingQueueEntryReadService;
            _listingQueueEntryWriteService = listingQueueEntryWriteService;
            _unitOfWork = unitOfWork;
            _listingReadService = listingReadService;
        }

        public async Task ExecuteAsync(Guid listingId, Guid listingQueueEntryId, UpdateListingQueueEntryPriorityRequest request, Guid requesterUserId)
        {
            Listing? listing = await _listingReadService.GetListingByIdAsync(listingId);
            if (listing == null)
            {
                throw new ListingNotFoundException(listingId);
            }
            if (listing.SellerUserId != requesterUserId)
            {
                throw new UnauthorizedAccessException();
            }
            ListingQueueEntryFilter filter = new ListingQueueEntryFilter()
            {
                ListingIds = [listingId],
            };

            IEnumerable<ListingQueueEntry> queue = await _listingQueueEntryReadService.GetFilteredListingQueueEntriesAsync(filter);

            // Validate index
            if (request.RequestedIndex < 0 || request.RequestedIndex > queue.Count())
            {
                throw new InvalidOperationException($"Index {request.RequestedIndex} not in range");
            }


            List<ListingQueueEntry> orderedPriorityQueue = queue.Where(qe => qe.Priority != null).OrderBy(qe => qe.Priority).ToList();
            ListingQueueEntry requestedEntry = orderedPriorityQueue.Single(qe => qe.Id == listingQueueEntryId);

            // Reorder requested entry in collection
            orderedPriorityQueue.Remove(requestedEntry);
            orderedPriorityQueue.Insert(request.RequestedIndex, requestedEntry);

            IEnumerable<ListingQueueEntry> normalizedQueue = orderedPriorityQueue.Normalize();

            // Update queue
            await _listingQueueEntryWriteService.UpdateListingQueueEntriesAsync(normalizedQueue);

        }
    }
}
