using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Dtos;
using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Mappers;
using BoardGameMarket.MarketGateway.API.Application.CatalogItems.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Services;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
    [Route("api/catalog")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogDataService _catalogService;

        public CatalogController(CatalogDataService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet("bgg/{bggId:int}")]
        public async Task<ActionResult<CatalogItemDto>> GetBggItemByBggId(int bggId)
        {
            CatalogItemInfo catalogItemInfo = await _catalogService.GetBggCatalogItemByBggId(bggId);
            return Ok(catalogItemInfo.ToCatalogItemDto());
        }

        [HttpPost("custom")]
        public async Task<ActionResult<CatalogItemDto>> GetCustomItemsByNames([FromBody] CustomCatalogItemsRequestDto request)
        {
            CatalogItemInfo[] items = await _catalogService.GetCustomCatalogItemsByNames(new CustomCatalogItemsRequest(request.Names));

            return Ok(items.Select(i => i.ToCatalogItemDto()));
        }
    }
}
