using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Aggregates;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.Queries;
using BoardGameMarket.ListingsService.Core.Domain.Aggragates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueue
{
	public class GetQueueUseCase
	{
		private readonly ListingQueueAggregateBuilder _listingQueueAggregateBuilder;
		private readonly ListingOwnershipQuery _listingOwnershipQuery;

        public GetQueueUseCase(ListingQueueAggregateBuilder listingQueueAggregateBuilder, ListingOwnershipQuery listingOwnershipQuery)
        {
            _listingQueueAggregateBuilder = listingQueueAggregateBuilder;
            _listingOwnershipQuery = listingOwnershipQuery;
        }

        public async Task<ListingQueueInfo> Execute(Guid listingId, Guid requesterUserId)
		{
			Guid sellerId = await _listingOwnershipQuery.GetSellerUserIdByListingIdAsync(listingId);
			if (sellerId != requesterUserId)
			{
				throw new UnauthorizedAccessException("Seller/Requester mismatch");
			}

			ListingQueueAggregate listingQueueAggregate = await _listingQueueAggregateBuilder.GetListingQueueAggregate(listingId);

			ListingQueueEntryInfo[] queueByTime = listingQueueAggregate.QueueByTime.Select(qe => qe.ToListingQueueEntryInfo()).ToArray();
			ListingQueueEntryInfo[] queueByPriority = listingQueueAggregate.QueueByPriority.Select(qe => qe.ToListingQueueEntryInfo()).ToArray();

			return new ListingQueueInfo(queueByTime, queueByPriority);
		}
	}
}
