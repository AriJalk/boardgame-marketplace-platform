using BoardGameMarket.ListingsService.API.Helpers;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingSales;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingSale;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.ListingsService.API.Controllers
{
    [Route("api/listing-sales")]
    [ApiController]
    public class ListingSalesController : ControllerBase
    {
        private readonly IListingSaleReadService _listingSaleReadService;

        public ListingSalesController(IListingSaleReadService listingSaleReadService)
        {
            _listingSaleReadService=listingSaleReadService;
        }

        [HttpGet("{listingSaleId:guid}")]
        public async Task<ActionResult<ListingSaleInfo>> GetListingSale(Guid listingSaleId, [FromHeader(Name = Headers.REQUESTER_USER_ID)] Guid requesterUserId)
        {
            ListingSale? listingSale = await _listingSaleReadService.GetListingSaleByIdAsync(listingSaleId);
            if (listingSale == null)
            {
                return NotFound();
            }
            if (listingSale.BuyerUserId != requesterUserId && listingSale.SellerUserId != requesterUserId)
            {
                return Unauthorized();
            }
            return Ok(listingSale.ToListingSaleInfo());
        }

        [HttpGet("by-user/{userId:guid}")]
        public async Task<ActionResult<ListingSale[]>> GetListingSalesByUser(Guid userId)
        {
            IEnumerable<ListingSale> listingSales = await _listingSaleReadService.GetListingSalesByUserIdAsync(userId);

            return Ok(listingSales.Select(ls => ls.ToListingSaleInfo()));
        }
    }
}
