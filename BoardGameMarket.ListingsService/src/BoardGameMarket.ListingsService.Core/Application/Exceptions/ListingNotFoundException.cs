using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Exceptions
{
    public class ListingNotFoundException : Exception
    {
        public Guid ListingId { get; init; }
        public ListingNotFoundException(Guid listingId) : base($"Listing {listingId} not found")
        {
            ListingId = listingId;
        }
        public ListingNotFoundException(string? message) : base(message) { }
    }
}
