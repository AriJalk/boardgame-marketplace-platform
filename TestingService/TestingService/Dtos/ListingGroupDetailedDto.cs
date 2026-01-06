namespace TestingService.Dtos
{
	public class ListingGroupDetailedDto
	{
		public Guid Id { get; set; }
		public UserSummaryDto? Seller { get; set; }
		public DateTime PostingTime { get; set; }
		public string? PickupOptions { get; set; }
		public string? Location { get; set; }
		public string? Details { get; set; }
		public IEnumerable<ListingDetailDto>? Listings { get; set; }
	}

}
