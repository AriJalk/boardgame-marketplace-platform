using BoardGameMarket.ListingsService.Core.Application.Access.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Aggregates;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Exceptions;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.Queries;
using BoardGameMarket.ListingsService.Core.Domain.Aggregates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueue
{
	public class GetQueueUseCase
	{
		private readonly ListingQueueAggregateBuilder _listingQueueAggregateBuilder;
		private readonly IListingReadService _listingReadService;

        public GetQueueUseCase(ListingQueueAggregateBuilder listingQueueAggregateBuilder, IListingReadService listingReadService)
        {
            _listingQueueAggregateBuilder = listingQueueAggregateBuilder;
            _listingReadService = listingReadService;
        }

        public async Task<ListingQueueInfo> ExecuteAsync(Guid listingId, Guid requesterUserId)
		{
			Listing? listing = await _listingReadService.GetListingByIdAsync(listingId);
			if (listing == null)
			{
                throw new ListingNotFoundException(listingId);
            }
			if (listing.SellerUserId != requesterUserId)
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
