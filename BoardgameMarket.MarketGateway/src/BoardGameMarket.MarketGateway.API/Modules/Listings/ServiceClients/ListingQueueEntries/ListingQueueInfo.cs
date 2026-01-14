using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.ListingQueueEntries
{
	public record ListingQueueInfo(ListingQueueEntryInfo[] QueueByTime, ListingQueueEntryInfo[] QueueByPriority)
	{
	}
}
