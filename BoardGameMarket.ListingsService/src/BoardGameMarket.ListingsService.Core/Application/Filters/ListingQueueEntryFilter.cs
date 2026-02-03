using BoardGameMarket.ListingsService.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Filters
{
	public class ListingQueueEntryFilter
	{
		public Guid? UserId { get; init; }
		public IReadOnlyCollection<Guid>? ListingIds { get; init; }
		public ListingStatus? ListingStatus { get; init; }
	}
}
