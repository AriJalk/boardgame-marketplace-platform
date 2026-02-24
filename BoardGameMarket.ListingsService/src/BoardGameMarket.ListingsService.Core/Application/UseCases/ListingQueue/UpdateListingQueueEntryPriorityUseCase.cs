using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
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
        private readonly ListingOwnershipQuery _listingOwnershipQuery;

        public UpdateListingQueueEntryPriorityUseCase(IListingQueueEntryReadService listingQueueEntryReadService, IListingQueueEntryWriteService listingQueueEntryWriteService, IUnitOfWork unitOfWork, ListingOwnershipQuery listingOwnershipQuery)
        {
            _listingQueueEntryReadService = listingQueueEntryReadService;
            _listingQueueEntryWriteService = listingQueueEntryWriteService;
            _unitOfWork = unitOfWork;
            _listingOwnershipQuery = listingOwnershipQuery;
        }

        public async Task<ListingQueueEntryInfo> ExecuteAsync(Guid listingId, Guid listingQueueEntryId, UpdateListingQueueEntryPriorityRequest request, Guid requesterUserId)
        {
            Guid sellerId = await _listingOwnershipQuery.GetSellerUserIdByListingIdAsync(listingId);
            if (sellerId != requesterUserId)
            {
                throw new UnauthorizedAccessException();
            }
            ListingQueueEntryFilter filter = new ListingQueueEntryFilter()
            {
                ListingIds = [listingId],
            };
            try
            {
                await _unitOfWork.BeginTransactionAsync();


                IEnumerable<ListingQueueEntry> queue = await _listingQueueEntryReadService.GetFilteredListingQueueEntriesAsync(filter);
                if (request.RequestedIndex < 0 || request.RequestedIndex > queue.Count())
                {
                    throw new InvalidOperationException($"Index {request.RequestedIndex} not in range");
                }
                List<ListingQueueEntry> orderedPriorityQueue = queue.Where(qe => qe.Priority != null).OrderBy(qe => qe.Priority).ToList();
                ListingQueueEntry requestedEntry = orderedPriorityQueue.Single(qe => qe.Id == listingQueueEntryId);

                orderedPriorityQueue.Remove(requestedEntry);
                orderedPriorityQueue.Insert(request.RequestedIndex, requestedEntry);

                IEnumerable<ListingQueueEntry> normalizedQueue = orderedPriorityQueue.Normalize();
                IList<ListingQueueEntry> updateResult = (await _listingQueueEntryWriteService.UpdateListingQueueEntriesAsync(normalizedQueue)).ToList();
                if (!updateResult.Any())
                {
                    throw new InvalidOperationException("Can't update queue");
                }

                await _unitOfWork.CommitTransactionAsync();

                return updateResult[request.RequestedIndex].ToListingQueueEntryInfo();
            }
            catch
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw;
            }
        }
    }
}
