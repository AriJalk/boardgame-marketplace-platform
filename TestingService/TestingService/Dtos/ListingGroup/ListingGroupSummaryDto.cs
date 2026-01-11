using TestingService.Dtos.Listing;
using TestingService.Dtos.User;

namespace TestingService.Dtos.ListingGroup
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
