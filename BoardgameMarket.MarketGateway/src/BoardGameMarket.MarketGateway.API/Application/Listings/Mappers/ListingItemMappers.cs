using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Dtos;
using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.ListingItems;
using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.ListingItems;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.Mappers
{
	public static class ListingItemMappers
	{
		public static ListingItemDto ToListingItemDto(this ListingItemInfo listingItemInfo, CatalogItemDto catalogItem)
		{
			return new ListingItemDto(listingItemInfo.Id, catalogItem, listingItemInfo.Details);
		}

		public static CreateListingItemRequest ToCreateListingItemRequest(this CreateListingItemRequestDto dto)
		{
			return new CreateListingItemRequest(dto.CatalogId, dto.Details);
		}

		public static UpdateListingItemRequest ToUpdateListingItemRequest(this UpdateListingItemRequestDto dto)
		{
			return new UpdateListingItemRequest(dto.Id, dto.CatalogId, dto.Details);
		}
	}
}
