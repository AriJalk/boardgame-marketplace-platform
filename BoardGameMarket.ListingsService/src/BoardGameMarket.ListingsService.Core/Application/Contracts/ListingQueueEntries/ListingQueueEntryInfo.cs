using BoardGameMarket.ListingsService.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries
{
	public record ListingQueueEntryInfo(Guid Id, Guid ListingId, QueueStatus Status, Guid BuyerUserId, DateTime CreatedAt, DateTime ModifiedAt, decimal? Priority)
	{
	}
}
