using BoardGameMarket.MarketGateway.API.Modules.Queues.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.ListingQueueEntries
{
	public record ListingQueueEntryInfo(Guid Id, QueueStatus Status, Guid BuyerUserId, DateTime CreatedAt, DateTime ModifiedAt, decimal? Priority)
	{
	}
}
