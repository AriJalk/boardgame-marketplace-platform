namespace BoardGameMarket.MarketGateway.API.Application.ListingGroups.Filters
{
	public class PagedListingGroupQuery
	{
		public int PageSize { get; set; } = 10;
		public string? Cursor { get; set; }
		public ListingGroupFilter? Filter { get; set; }
	}
}
