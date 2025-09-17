using BoardGameMarket.BggCatalogService.Core.Application.DTOs;
using BoardGameMarket.BggCatalogService.Core.Application.ServiceContracts;
using BoardGameMarket.BggCatalogService.Core.Application.UseCases;
using BoardGameMarket.BggCatalogService.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.BggCatalogService.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ItemsController : ControllerBase
	{
		private readonly IBggItemGetterService _bggItemGetterService;
		private readonly BggItemResponseMapperUseCase _bggItemResponseMapperUseCase;

		public ItemsController(IBggItemGetterService bggItemGetterService, BggItemResponseMapperUseCase bggItemResponseMapperUseCase)
		{
			_bggItemGetterService = bggItemGetterService;
			_bggItemResponseMapperUseCase = bggItemResponseMapperUseCase;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<BggItemResponse>>> GetItemsDataByBggId([FromQuery] int[] bggIds)
		{
			if (!bggIds.Any())
				return BadRequest("No IDs provided.");

			IEnumerable<BggItem> items = await _bggItemGetterService.GetItemsByBggIds(bggIds);
			return Ok(await _bggItemResponseMapperUseCase.MapToResponseAsync(items));
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<BggItemResponse>>> GetItemsDataByDatabaseId([FromQuery] Guid[] dbIds)
		{
			if (!dbIds.Any())
				return BadRequest("No IDs provided.");

			IEnumerable<BggItem> items = await _bggItemGetterService.GetItemsByDatabaseIds(dbIds);
			return Ok(await _bggItemResponseMapperUseCase.MapToResponseAsync(items));
		}

	}
}
