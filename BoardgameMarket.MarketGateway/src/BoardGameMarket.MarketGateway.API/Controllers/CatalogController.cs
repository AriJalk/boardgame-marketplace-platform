using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Dtos;
using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Mappers;
using BoardGameMarket.MarketGateway.API.Application.CatalogItems.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.CatalogItems.Services;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
	[Route("api/[controller]")]
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
	}
}
