using Microsoft.AspNetCore.Mvc;
using BoardGameMarket.MarketGateway.API.Modules.CatalogItems.Dtos;
using BoardGameMarket.MarketGateway.API.Modules.CatalogItems.Helpers;
using BoardGameMarket.MarketGateway.API.Modules.CatalogItems.ServiceClients;
using BoardGameMarket.MarketGateway.API.Modules.CatalogItems.Services;
using BoardGameMarket.MarketGateway.API.Modules.Listings.Dtos;
using BoardGameMarket.MarketGateway.API.Modules.Listings.Helpers;
using BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.ListingGroups;
using BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.ListingItems;
using BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.Listings;
using BoardGameMarket.MarketGateway.API.Modules.Listings.Services;
using BoardGameMarket.MarketGateway.API.Modules.Users.Helpers;
using BoardGameMarket.MarketGateway.API.Modules.Users.ServiceClients;
using BoardGameMarket.MarketGateway.API.Modules.Users.Services;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingGroupsController : ControllerBase
	{
		ListingsDataService _listingsService;
		CatalogService _catalogService;
		UsersService _usersService;

		public ListingGroupsController(ListingsDataService listingsService, CatalogService catalogService, UsersService usersService)
		{
			_listingsService = listingsService;
			_catalogService = catalogService;
			_usersService = usersService;
		}

		[HttpGet("{id:guid}/detail")]
		public IActionResult GetListingGroupDetail(Guid id)
		{
			if (_listingsService.ListingGroups.TryGetValue(id, out ListingGroupDetail? lg))
			{
				return Ok(BuildListingGroupDetailDto(lg));
			}

			return NotFound();
		}

		private ListingGroupDetailDto BuildListingGroupDetailDto(ListingGroupDetail listingGroup)
		{
			List<ListingDetailDto> listingDtos = new List<ListingDetailDto>();
			foreach (ListingInGroupDetail listing in listingGroup.Listings)
			{
				listingDtos.Add(BuildListingDetailDto(listing));
			}
			UserInfo sellerUserInfo = _usersService.Users[listingGroup.SellerUserId];
			return new ListingGroupDetailDto()
			{
				Id = listingGroup.Id,
				Seller = sellerUserInfo.ToUserSummaryDto(),
				PostingTime = listingGroup.CreatedAt,
				Location = listingGroup.Location,
				PickupOptions = listingGroup.PickupOptions,
				Details = listingGroup.Details,
				Listings = listingDtos,
			};
		}

		private ListingDetailDto BuildListingDetailDto(ListingInGroupDetail listing)
		{
			List<ListingItemDto> listingItemDtos = new List<ListingItemDto>();
			foreach (ListingItemInfo listingItemInfo in listing.ListingItems)
			{
				listingItemDtos.Add(BuildListingItemDto(listingItemInfo));
			}
			return new ListingDetailDto()
			{
				Id = listing.Id,
				Title = listing.Title,
				Price = listing.Price,
				Details = listing.Details,
				Items = listingItemDtos
			};
		}

		private ListingItemDto BuildListingItemDto(ListingItemInfo listingItem)
		{
			CatalogItemDto catalogItemDto = new CatalogItemDto();
			if (_catalogService.CatalogItems.TryGetValue(listingItem.CatalogId, out CatalogItemInfo? catalogItemInfo))
			{
				catalogItemDto = catalogItemInfo.ToCatalogItemDto();
			}
			return new ListingItemDto()
			{
				Id = listingItem.Id,
				CatalogItem = catalogItemDto,
				Details = listingItem.Details,
			};
		}

		[HttpGet("{id:guid}/summary")]
		public IActionResult GetListingGroupSummary(Guid id)
		{
			if (_listingsService.ListingGroups.TryGetValue(id, out ListingGroupDetail? lg))
			{
				return Ok(BuildListingGroupSummaryDto(lg));
			}
			return NotFound();
		}

		private ListingGroupSummaryDto BuildListingGroupSummaryDto(ListingGroupDetail listingGroup)
		{
			List<ListingSummaryDto> listingSummaryDtos = new List<ListingSummaryDto>();
			foreach (ListingInGroupDetail listing in listingGroup.Listings)
			{
				listingSummaryDtos.Add(listing.ToListingSummaryDto());
			}
			UserInfo sellerUserInfo = _usersService.Users[listingGroup.SellerUserId];
			return new ListingGroupSummaryDto()
			{
				Id = listingGroup.Id,
				Seller = sellerUserInfo.ToUserSummaryDto(),
				PostingTime = listingGroup.CreatedAt,
				Location = listingGroup.Location,
				PickupOptions = listingGroup.PickupOptions,
				Details = listingGroup.Details,
				Listings = listingSummaryDtos
			};
		}



		[HttpGet()]
		public ActionResult<IEnumerable<ListingGroupSummaryDto>> Get()
		{
			return Ok(_listingsService.ListingGroups.Values.Select(lg => BuildListingGroupSummaryDto(lg)));
		}
	}
}
