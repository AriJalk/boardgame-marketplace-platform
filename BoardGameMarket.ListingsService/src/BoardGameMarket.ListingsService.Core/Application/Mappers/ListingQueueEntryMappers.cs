using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Mappers
{
	public static class ListingQueueEntryMappers
	{
		public static ListingQueueEntryInfo ToListingQueueEntryInfo(this ListingQueueEntry listingQueueEntry)
		{
			return new ListingQueueEntryInfo(listingQueueEntry.Id, listingQueueEntry.Status, listingQueueEntry.BuyerUserId, listingQueueEntry.CreatedAt, listingQueueEntry.ModifiedAt, listingQueueEntry.Priority);
		}
	}
}
