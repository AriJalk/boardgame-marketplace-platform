using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.MarketGateway.API.Application.ListingQueues.ServiceClients
{
	public record ListingQueueInfo(ListingQueueEntryInfo[] QueueByTime, ListingQueueEntryInfo[] QueueByPriority)
	{
	}
}
