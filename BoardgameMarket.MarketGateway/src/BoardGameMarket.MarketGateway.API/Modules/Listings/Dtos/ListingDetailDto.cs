namespace BoardGameMarket.MarketGateway.API.Modules.Listings.Dtos
{
	public class ListingDetailDto
	{
		public Guid Id { get; set; }
		public string? Title { get; set; }
		public decimal Price { get; set; }
		public string? Details { get; set; }
		public IEnumerable<ListingItemDto> Items { get; set; } = Enumerable.Empty<ListingItemDto>();
	}
}
