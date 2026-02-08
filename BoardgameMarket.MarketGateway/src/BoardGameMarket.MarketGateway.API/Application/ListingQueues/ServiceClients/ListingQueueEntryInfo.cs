using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.MarketGateway.API.Application.ListingQueues.ServiceClients
{
	public record ListingQueueEntryInfo(Guid Id, Guid ListingId, QueueStatus Status, Guid BuyerUserId, DateTime CreatedAt, DateTime ModifiedAt, decimal? Priority)
	{
	}
}
