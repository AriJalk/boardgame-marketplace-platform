using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Domain.Aggragates
{
	public record ListingQueueAggregate(IReadOnlyCollection<ListingQueueEntry> QueueByTime, IReadOnlyCollection<ListingQueueEntry> QueueByPriority)
	{ }
}
