using BoardGameMarket.MarketGateway.API.Application.Listings.Assemblers;
using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Application.Listings.Mappers;
using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.Listings;
using BoardGameMarket.MarketGateway.API.Application.Listings.Services;
using BoardGameMarket.MarketGateway.API.Application.Users.Mappers;
using BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
	[Route("api/listings")]
	[ApiController]
	public class ListingsController : ControllerBase
	{
		private readonly ListingsDataService _listingsDataService;
        private readonly ListingDetailDtoAssembler _listingDetailDtoAssembler;
        private readonly UsersDataService _usersDataService;

        public ListingsController(ListingsDataService listingsDataService, ListingDetailDtoAssembler listingDetailDtoAssembler, UsersDataService usersDataService)
        {
            _listingsDataService = listingsDataService;
            _listingDetailDtoAssembler = listingDetailDtoAssembler;
            _usersDataService = usersDataService;
        }

        [HttpGet("{listingId:guid}")]
        public async Task<ActionResult<ListingDetailDto>> GetListing(Guid listingId)
        {
            ListingDetail listing = await _listingsDataService.GetListing(listingId);
            UserInfo seller = await _usersDataService.GetUserOrThrow(listing.SellerUserId);

            ListingDetailDto dto = await _listingDetailDtoAssembler.BuildListingDetailDto(listing, seller.ToUserSummaryDto());

            return Ok(dto);
        }
    }
}
