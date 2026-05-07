using BoardGameMarket.ListingsService.Core.Application.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.Contracts.ListingGroups
{
    public class PagedListingGroupQuery
    {
        [Range(1, 100, MaximumIsExclusive = false)]
        public int PageSize { get; set; } = 10;
        public string? Cursor { get; set; }
        public ListingGroupFilter? Filter { get; set; }
    }
}
