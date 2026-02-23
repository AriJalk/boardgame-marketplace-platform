using BoardGameMarket.CatalogService.Core.Application.Contracts;
using BoardGameMarket.CatalogService.Core.Application.Mappers;
using BoardGameMarket.CatalogService.Core.Application.ServiceContracts;
using BoardGameMarket.CatalogService.Core.Application.UseCases;
using BoardGameMarket.CatalogService.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.CatalogService.API.Controllers
{
	[Route("api/catalog")]
	[ApiController]
	public class CatalogController : ControllerBase
	{
		private readonly ICatalogItemGetterService _catalogItemGetterService;
		private readonly GetBggItemUseCase _getBggItemUseCase;
		private readonly GetCustomItemUseCase _getCustomItemUseCase;
		private readonly GetCustomItemsUseCase _getCustomItemsUseCase;

        public CatalogController(ICatalogItemGetterService catalogItemGetterService, GetBggItemUseCase getBggItemUseCase, GetCustomItemUseCase getCustomItemUseCase, GetCustomItemsUseCase getCustomItemsUseCase)
        {
            _catalogItemGetterService = catalogItemGetterService;
            _getBggItemUseCase = getBggItemUseCase;
            _getCustomItemUseCase = getCustomItemUseCase;
            _getCustomItemsUseCase = getCustomItemsUseCase;
        }

        [HttpGet("bgg/{bggId}")]
		public async Task<ActionResult<CatalogItemInfo>> GetBggItemByBggId(int bggId)
		{
			CatalogItemInfo catalogItemInfo = await _getBggItemUseCase.Execute(bggId);
			return Ok(catalogItemInfo);
		}

		[HttpGet("{catalogItemId:guid}")]
		public async Task<ActionResult<CatalogItemInfo>> GetCatalogItemById(Guid catalogItemId)
		{
			CatalogItem? catalogItem = await _catalogItemGetterService.GetCatalogItemByInternalIdASync(catalogItemId);

			if (catalogItem == null)
			{
				return NotFound();
			}

			return Ok(catalogItem.ToCatalogItemInfo());
		}

		[HttpGet("custom/{name}")]
		public async Task<ActionResult<CatalogItemInfo>> GetCustomCatalogItem(string name)
		{
			CatalogItemInfo catalogItemInfo = await _getCustomItemUseCase.Execute(name);
			return Ok(catalogItemInfo);
		}

		[HttpPost("custom")]
		public async Task<ActionResult<CatalogItemInfo[]>> GetCustomCatalogItems([FromBody] CustomCatalogItemsRequest request)
		{
			IReadOnlyCollection<CatalogItemInfo> items = await _getCustomItemsUseCase.Execute(request.Names);
			return Ok(items);
		}

		[HttpGet()]
		public async Task<ActionResult<IEnumerable<CatalogItemInfo>>> Get()
		{
			IEnumerable<CatalogItemInfo> items = (await _catalogItemGetterService.GetCatalogItemsAsync()).Select(i => i.ToCatalogItemInfo());
			return Ok(items);
		}
	}
}
