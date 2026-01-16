using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Mappers;
using BoardGameMarket.MarketGateway.API.Application.CatalogItems.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Services;
using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.ListingItems;
using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Application.Listings.Mappers;
using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.ListingItems;
using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.Listings;
using System.Reflection;
using System.Threading.Tasks;

namespace BoardGameMarket.MarketGateway.API.Application.Listings.Assemblers
{
	public class ListingDetailDtoAssembler
	{
		private readonly CatalogDataService _catalogService;

		public ListingDetailDtoAssembler(CatalogDataService catalogService)
		{
			_catalogService = catalogService;
		}

		private async Task<IEnumerable<ListingItemDto>> BuildListingItemDtos(IEnumerable<ListingItemInfo> listingItems)
		{
			IEnumerable<Task<ListingItemDto>> tasks = listingItems.Select(async item =>
			{
				CatalogItemInfo catalogItem = await _catalogService.GetCatalogItemByInternalId(item.CatalogId);
				return item.ToListingItemDto(catalogItem.ToCatalogItemDto());
			});

			return await Task.WhenAll(tasks);
		}

		public async Task<ListingDetailDto> BuildListingDetailDto(ListingDetail listing)
		{
			IEnumerable<ListingItemDto> listingItemDtos = await BuildListingItemDtos(listing.ListingItems);
			return listing.ToListingDetailDto(listingItemDtos);
			
		}

		public async Task<ListingDetailDto> BuildListingDetailDto(ListingInGroupDetail listing)
		{
			IEnumerable<ListingItemDto> listingItemDtos = await BuildListingItemDtos(listing.ListingItems);
			return listing.ToListingDetailDto(listingItemDtos);
		}
	}
}
