using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Exceptions
{
    internal class ListingNotFoundException : Exception
    {
        public ListingNotFoundException(Guid listingId) : base($"Listing {listingId} not found")
        { }
    }
}
