using BoardGameMarket.ListingsService.Core.Domain.Enums;

namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings
{
	public class ListingUpdateRequest
	{
		public Guid Id { get; set; }
		public Guid ListingGroupId { get; set; }
		public ListingStatus Status { get; set; }
		public decimal Price { get; set; }
		public string? Details { get; set; }
	}
}
