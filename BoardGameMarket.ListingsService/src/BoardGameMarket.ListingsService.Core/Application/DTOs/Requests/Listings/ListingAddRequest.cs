using BoardGameMarket.ListingsService.Core.Domain.Enums;

namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings
{
	public class ListingAddRequest
	{
		public Guid ListingGroupId { get; set; }
		public ListingStatus Status { get; set; }
		public decimal Price { get; set; }
		public string? Details { get; set; }
	}
}
