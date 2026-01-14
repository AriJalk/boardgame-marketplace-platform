using BoardGameMarket.MarketGateway.API.Modules.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Modules.Users.Dtos;

namespace BoardGameMarket.MarketGateway.API.Modules.Listings.Dtos.ListingGroups
{
	public class ListingGroupDetailDto
	{
		public Guid Id { get; set; }
		public UserSummaryDto? Seller { get; set; }
		public DateTime PostingTime { get; set; }
		public string? PickupOptions { get; set; }
		public string? Location { get; set; }
		public string? Details { get; set; }
		public IEnumerable<ListingDetailDto> Listings { get; set; } = Enumerable.Empty<ListingDetailDto>();
	}

}
