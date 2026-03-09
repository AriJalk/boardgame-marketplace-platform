using BoardGameMarket.ListingsService.API.Helpers;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingsSold;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingSold;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.ListingsService.API.Controllers
{
    [Route("api/listings-sold")]
    [ApiController]
    public class ListingsSoldController : ControllerBase
    {
        private readonly IListingSoldReadService _listingSoldReadService;

        public ListingsSoldController(IListingSoldReadService listingSoldReadService)
        {
            _listingSoldReadService=listingSoldReadService;
        }

        [HttpGet("{listingSoldId:guid}")]
        public async Task<ActionResult<ListingSoldInfo>> GetListingSold(Guid listingSoldId, [FromHeader(Name = Headers.REQUESTER_USER_ID)] Guid requesterUserId)
        {
            ListingSold? listingSold = await _listingSoldReadService.GetListingSoldByIdAsync(listingSoldId);
            if (listingSold == null)
            {
                return NotFound();
            }
            if (listingSold.BuyerUserId != requesterUserId && listingSold.SellerUserId != requesterUserId)
            {
                return Unauthorized();
            }
            return Ok(listingSold.ToListingSoldInfo());
        }

        [HttpGet("by-user/{userId:guid}")]
        public async Task<ActionResult<ListingSold[]>> GetListingsSoldByUser(Guid userId)
        {
            IEnumerable<ListingSold> listingsSold = await _listingSoldReadService.GetListingsSoldByUserIdAsync(userId);

            return Ok(listingsSold.Select(ls => ls.ToListingSoldInfo()));
        }
    }
}
