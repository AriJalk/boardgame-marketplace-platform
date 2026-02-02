using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Domain.Aggragates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Aggregates
{
	public class ListingQueueAggregateBuilder
	{
		private IListingQueueEntryReadService _listingQueueEntryReadService;

		public ListingQueueAggregateBuilder(IListingQueueEntryReadService listingQueueEntryReadService)
		{
			_listingQueueEntryReadService = listingQueueEntryReadService;
		}

		public async Task<ListingQueueAggregate> GetListingQueueAggregate(Guid listingId)
		{
			IEnumerable<ListingQueueEntry> allEntries = await _listingQueueEntryReadService.GetListingQueueEntriesByListingIdAsync(listingId);

			IReadOnlyCollection<ListingQueueEntry> queueByTime = allEntries.OrderBy(qe => qe.CreatedAt).ToList().AsReadOnly();

			IReadOnlyCollection<ListingQueueEntry> queueByPriority = allEntries.Where(qe => qe.Priority != null).OrderBy(qe => qe.Priority).ToList().AsReadOnly();

			return new ListingQueueAggregate(queueByTime, queueByPriority);
		}
	}
}
