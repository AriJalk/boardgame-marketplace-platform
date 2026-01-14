using BoardGameMarket.MarketGateway.API.Modules.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Modules.Users.Dtos;

namespace BoardGameMarket.MarketGateway.API.Modules.Listings.Dtos.ListingGroups
{
	public class ListingGroupSummaryDto
	{
		public Guid Id { get; set; }
		public UserSummaryDto? Seller { get; set; }
		public DateTime PostingTime { get; set; }
		public string? PickupOptions { get; set; }
		public string? Location { get; set; }
		public string? Details { get; set; }
		public IEnumerable<ListingSummaryDto> Listings { get; set; } = Enumerable.Empty<ListingSummaryDto>();
	}

}
