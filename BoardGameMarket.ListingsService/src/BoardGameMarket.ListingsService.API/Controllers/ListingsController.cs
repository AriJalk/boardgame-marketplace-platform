using BoardGameMarket.ListingsService.Core.Application.Contracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.ListingsService.API.Controllers
{
    [Route("api/listings")]
    [ApiController]
    public class ListingsController : ControllerBase
    {
        private readonly ListingOwnershipQuery _listingOwnershipQuery;

        public ListingsController(ListingOwnershipQuery listingOwnershipQuery)
        {
            _listingOwnershipQuery = listingOwnershipQuery;
        }

        [HttpGet("{listingId}/ownership")]
        public async Task<ActionResult<ListingOwnershipInfo>> GetListingOwnershipInfo(Guid listingId)
        {
            Guid userId = await _listingOwnershipQuery.GetSellerUserIdByListingIdAsync(listingId);

            return Ok(new ListingOwnershipInfo(listingId, userId));
        }
    }
}
