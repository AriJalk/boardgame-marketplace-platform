using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Commands
{
	public record CreateListingQueueEntryCommand(Guid ListingId, Guid UserId)
	{
	}
}
