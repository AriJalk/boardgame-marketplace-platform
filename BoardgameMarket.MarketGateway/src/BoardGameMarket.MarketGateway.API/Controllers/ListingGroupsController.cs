using BoardGameMarket.MarketGateway.API.Modules.CatalogItems.Dtos;
using BoardGameMarket.MarketGateway.API.Modules.CatalogItems.Helpers;
using BoardGameMarket.MarketGateway.API.Modules.CatalogItems.ServiceClients;
using BoardGameMarket.MarketGateway.API.Modules.CatalogItems.Services;
using BoardGameMarket.MarketGateway.API.Modules.Common;
using BoardGameMarket.MarketGateway.API.Modules.Listings.Dtos.ListingGroups;
using BoardGameMarket.MarketGateway.API.Modules.Listings.Dtos.ListingItems;
using BoardGameMarket.MarketGateway.API.Modules.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Modules.Listings.Filters;
using BoardGameMarket.MarketGateway.API.Modules.Listings.Helpers;
using BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.ListingGroups;
using BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.ListingItems;
using BoardGameMarket.MarketGateway.API.Modules.Listings.ServiceClients.Listings;
using BoardGameMarket.MarketGateway.API.Modules.Listings.Services;
using BoardGameMarket.MarketGateway.API.Modules.Users.Helpers;
using BoardGameMarket.MarketGateway.API.Modules.Users.ServiceClients;
using BoardGameMarket.MarketGateway.API.Modules.Users.Services;
using Microsoft.AspNetCore.Mvc;

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
			return Ok();
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
			return Ok();
		}


		private ListingSummaryDto BuildListingSummaryDto(ListingInGroupSummary listing)
		{
			return new ListingSummaryDto(listing.Id, listing.Title, listing.Price, listing.Details);
		}

		private ListingGroupSummaryDto BuildListingGroupSummaryDto(ListingGroupSummary listingGroup)
		{
			UserInfo sellerUserInfo = new UserInfo(Guid.NewGuid(), Modules.Users.Enums.RoleType.User, "FIRST", "LAST", "DNUSER");

			IEnumerable<ListingSummaryDto> listings = listingGroup.Listings.Select(l => BuildListingSummaryDto(l));

			return new ListingGroupSummaryDto(listingGroup.Id, sellerUserInfo.ToUserSummaryDto(), listingGroup.CreatedAt, listingGroup.PickupOptions, listingGroup.Location, listingGroup.Details, listings);
		}


		[HttpGet()]
		public async Task<ActionResult<IEnumerable<ListingGroupSummaryDto>>> Get([FromQuery] string? cursor = null, [FromQuery] int pageSize = 10, [FromQuery] ListingGroupFilter? filter = null)
		{
			PagedResult<ListingGroupSummary> pagedResult = await _listingsService.GetListingGroupsSummaryPage(pageSize, cursor, filter);

			IEnumerable<ListingGroupSummaryDto> listingGroups = pagedResult.Items.Select(i => BuildListingGroupSummaryDto(i));

			return Ok(listingGroups);
		}
	}
}
