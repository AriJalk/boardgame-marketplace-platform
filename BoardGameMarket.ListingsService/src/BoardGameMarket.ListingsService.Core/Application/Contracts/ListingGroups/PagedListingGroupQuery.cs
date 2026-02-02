using BoardGameMarket.ListingsService.Core.Application.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.ListingGroups
{
	public class PagedListingGroupQuery
	{
		public int PageSize { get; set; } = 10;
		public string? Cursor { get; set; }
		public ListingGroupFilter? Filter { get; set; }
	}
}
