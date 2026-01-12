using TestingService.Domains.Users.Dtos;

namespace TestingService.Domains.Listings.Dtos
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
