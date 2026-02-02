using BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Queries
{
	public class SubscriptionQueries
	{
		private readonly IListingQueueEntryReadService _listingQueueEntryReadService;

		public SubscriptionQueries(IListingQueueEntryReadService listingQueueEntryReadService)
		{
			_listingQueueEntryReadService = listingQueueEntryReadService;
		}

		public async Task<IReadOnlyCollection<Guid>> GetListingsBySubscribedUser(Guid userId)
		{
			IEnumerable<ListingQueueEntry> queueEntries = await _listingQueueEntryReadService.GetListingQueueEntriesBySubscribedUserIdAsync(userId);

			return queueEntries.Select(qe => qe.ListingId).ToList().AsReadOnly();
		}
	}
}
