using System.ComponentModel.DataAnnotations;

namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.Filters
{
    public class PagedListingGroupQuery
    {
        [Range(1, 100, MaximumIsExclusive = false)]
        public int PageSize { get; set; } = 10;
        public string? Cursor { get; set; }
        public ListingGroupFilter? Filter { get; set; }
    }
}
