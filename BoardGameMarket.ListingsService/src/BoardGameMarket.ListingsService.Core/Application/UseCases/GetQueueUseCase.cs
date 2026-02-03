using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Aggregates;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Domain.Aggragates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases
{
	public class GetQueueUseCase
	{
		private readonly ListingQueueAggregateBuilder _listingQueueAggregateBuilder;

		public GetQueueUseCase(ListingQueueAggregateBuilder listingQueueAggregateBuilder)
		{
			_listingQueueAggregateBuilder = listingQueueAggregateBuilder;
		}

		public async Task<ListingQueueInfo> Execute(Guid listingId)
		{
			ListingQueueAggregate listingQueueAggregate = await _listingQueueAggregateBuilder.GetListingQueueAggregate(listingId);

			ListingQueueEntryInfo[] queueByTime = listingQueueAggregate.QueueByTime.Select(qe => qe.ToListingQueueEntryInfo()).ToArray();
			ListingQueueEntryInfo[] queueByPriority = listingQueueAggregate.QueueByPriority.Select(qe => qe.ToListingQueueEntryInfo()).ToArray();

			return new ListingQueueInfo(queueByTime, queueByPriority);
		}
	}
}
