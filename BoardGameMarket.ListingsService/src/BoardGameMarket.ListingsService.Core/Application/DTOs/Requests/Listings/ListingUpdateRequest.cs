using BoardGameMarket.ListingsService.Core.Domain.Enums;

namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings
{
	public class ListingUpdateRequest
	{
		public decimal Price { get; set; }
		public string? Details { get; set; }
	}
}
