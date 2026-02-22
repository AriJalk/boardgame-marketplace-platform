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

		public CatalogController(ICatalogItemGetterService catalogItemGetterService, GetBggItemUseCase getBggItemUseCase, GetCustomItemUseCase getCustomItemUseCase)
		{
			_catalogItemGetterService = catalogItemGetterService;
			_getBggItemUseCase = getBggItemUseCase;
			_getCustomItemUseCase = getCustomItemUseCase;
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

		[HttpGet()]
		public async Task<ActionResult<IEnumerable<CatalogItemInfo>>> Get()
		{
			IEnumerable<CatalogItemInfo> items = (await _catalogItemGetterService.GetCatalogItemsAsync()).Select(i => i.ToCatalogItemInfo());
			return Ok(items);
		}
	}
}
