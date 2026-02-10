using BoardGameMarket.MarketGateway.API.Application.ListingQueues.Dtos;
using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.ListingItems;
using BoardGameMarket.MarketGateway.API.Application.Listings.Enums;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings
{
	public record ListingDetailDto(Guid Id, string? Title, decimal Price, string? Details, ListingItemDto[] Items, ListingStatus Status, ListingSubscriptionResponseDto? Subscription)
	{
		public bool IsSubscribed => Subscription != null;
	}
}
