using BoardGameMarket.MarketGateway.API.Application.Listings.Assemblers;
using BoardGameMarket.MarketGateway.API.Application.Listings.Dtos.Listings;
using BoardGameMarket.MarketGateway.API.Application.Listings.Mappers;
using BoardGameMarket.MarketGateway.API.Application.Listings.ServiceClients.Listings;
using BoardGameMarket.MarketGateway.API.Application.Listings.Services;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
	[Route("api/listings")]
	[ApiController]
	public class ListingsController : ControllerBase
	{
		private readonly ListingsDataService _listingsDataService;
        private readonly ListingDetailDtoAssembler _listingDetailDtoAssembler;

        public ListingsController(ListingsDataService listingsDataService, ListingDetailDtoAssembler listingDetailDtoAssembler)
        {
            _listingsDataService = listingsDataService;
            _listingDetailDtoAssembler = listingDetailDtoAssembler;
        }

        [HttpGet("{listingId:guid}")]
        public async Task<ActionResult<ListingDetailDto>> GetListing(Guid listingId)
        {
            ListingDetail listing = await _listingsDataService.GetListing(listingId);

            ListingDetailDto dto = await _listingDetailDtoAssembler.BuildListingDetailDto(listing);

            return Ok(dto);
        }
    }
}
