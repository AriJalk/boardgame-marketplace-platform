using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Domain.Aggragates
{
	public class ListingAggregate
	{
		public Listing Listing { get; }

		private readonly IReadOnlyCollection<ListingItem>? _items;
		public IReadOnlyCollection<ListingItem> Items => _items ?? throw new InvalidOperationException("Listing items not loaded");

		private readonly ListingQueueAggregate? _queue;
		public ListingQueueAggregate Queue => _queue ?? throw new InvalidOperationException("Queue not loaded");

		public ListingAggregate(Listing listing, IReadOnlyCollection<ListingItem>? items = null, ListingQueueAggregate? queue = null)
		{
			Listing = listing;
			_items = items;
			_queue = queue;
		}
	}
}
