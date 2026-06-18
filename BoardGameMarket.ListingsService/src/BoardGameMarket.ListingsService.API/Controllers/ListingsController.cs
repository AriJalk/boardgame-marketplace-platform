using BoardGameMarket.ListingsService.Core.Application.Access.Listings;
using BoardGameMarket.ListingsService.Core.Application.Aggregates;
using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.Queries;
using BoardGameMarket.ListingsService.Core.Domain.Aggregates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.ListingsService.API.Controllers
{
    [Route("api/listings")]
    [ApiController]
    public class ListingsController : ControllerBase
    {
        private readonly IListingReadService _listingsReadService;
        private readonly ListingAggregateBuilder _listingAggregateBuilder;

        public ListingsController(IListingReadService listingsReadService, ListingAggregateBuilder listingAggregateBuilder)
        {
            _listingsReadService = listingsReadService;
            _listingAggregateBuilder = listingAggregateBuilder;
        }

        [HttpGet("{listingId:guid}/ownership")]
        public async Task<ActionResult<ListingOwnershipInfo>> GetListingOwnershipInfo(Guid listingId)
        {
            Listing? listing = await _listingsReadService.GetListingByIdAsync(listingId);
            if (listing == null)
            {
                return NotFound();
            }
            return Ok(new ListingOwnershipInfo(listingId, listing.SellerUserId));
        }

        [HttpGet("{listingId:guid}")]
        public async Task<ActionResult<ListingDetail>> GetListing(Guid listingId)
        {
            Listing? listing = await _listingsReadService.GetListingByIdAsync(listingId);
            if (listing == null)
            {
                return NotFound();
            }

            ListingAggregate listingAggregate = await _listingAggregateBuilder.BuildListingAggregateAsync(listing, true, false);
            return Ok(listingAggregate.ToListingDetail());
        }
    }
}
