namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Responses
{
	internal class ListingGroupResponseDto
	{
		public Guid Id { get; set; }
		public Guid SellerUserId { get; set; }
		public string? Details { get; set; }
		public string? PickupOptions { get; set; }
		public string? Location { get; set; }
		public IEnumerable<ListingResponseDto>? Listings { get; set; }
	}
}
