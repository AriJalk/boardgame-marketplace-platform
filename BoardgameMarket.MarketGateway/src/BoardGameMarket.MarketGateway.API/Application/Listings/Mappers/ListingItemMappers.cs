using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Dtos;
using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Mappers;
using BoardGameMarket.MarketGateway.API.Application.CatalogItems.ServiceClients;
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
	}
}
