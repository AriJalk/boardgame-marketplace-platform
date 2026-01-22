using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries
{
	public record AddListingQueueEntryRequest(Guid UserId) { }
}
