using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries
{
	public record ListingQueueInfo(ListingQueueEntryInfo[] QueueByTime, ListingQueueEntryInfo[] QueueByPriority)
	{
	}
}
