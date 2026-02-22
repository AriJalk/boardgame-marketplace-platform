using BoardGameMarket.ListingsService.Core.Application.Queries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.ListingsService.API.Controllers
{
    [Route("api/query")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly ListingOwnershipQuery _listingOwnershipQuery;

        public QueryController(ListingOwnershipQuery listingOwnershipQuery)
        {
            _listingOwnershipQuery = listingOwnershipQuery;
        }

        [HttpGet("seller/by-listing/{listingId:guid}")]
        public async Task<ActionResult<Guid>> GetSellerIdByListingId(Guid listingId)
        {
            Guid sellerId = await _listingOwnershipQuery.GetSellerUserIdByListingId(listingId);
            return Ok(sellerId);
        }

        [HttpGet("seller/by-listing-group-id/{listingGroupId:guid}")]
        public async Task<ActionResult<Guid>> GetSellerIdByListingGroupId(Guid listingGroupId)
        {
            Guid sellerId = await _listingOwnershipQuery.GetSellerUserIdByListingGroupId(listingGroupId);
            return Ok(sellerId);
        }

    }
}
