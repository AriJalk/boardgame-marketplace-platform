using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Domain.Aggragates
{
	public class ListingGroupAggregate
	{
		public ListingGroup ListingGroup { get; }

		private readonly IReadOnlyCollection<ListingAggregate>? _listings;
		public IReadOnlyCollection<ListingAggregate> Listings => _listings ?? throw new InvalidOperationException("Listings not loaded");

		internal ListingGroupAggregate(ListingGroup listingGroup, IReadOnlyCollection<ListingAggregate>? listings = null)
		{
			ListingGroup = listingGroup;
			_listings = listings;
		}
	}
}
