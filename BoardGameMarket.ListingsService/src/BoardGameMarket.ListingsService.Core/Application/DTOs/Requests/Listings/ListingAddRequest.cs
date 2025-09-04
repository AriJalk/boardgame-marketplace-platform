using BoardGameMarket.ListingsService.Core.Domain.Enums;

namespace BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Listings
{
	public record ListingAddRequest(Guid ListingGroupId, ListingStatus Status, decimal Price, string? Details) { }
}
