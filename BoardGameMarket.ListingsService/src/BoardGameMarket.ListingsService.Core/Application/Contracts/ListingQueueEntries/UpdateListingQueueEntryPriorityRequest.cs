using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries
{
    public record UpdateListingQueueEntryPriorityRequest(int RequestedIndex)
    {
    }
}
