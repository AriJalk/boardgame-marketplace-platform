using BoardGameMarket.GamesService.Core.Application.DTOs;
using BoardGameMarket.GamesService.Core.Application.ServiceContracts;
using BoardGameMarket.GamesService.Core.Domain.Entities;
using BoardGameMarket.GamesService.Core.Domain.RepositoryContracts;
using BoardGameMarket.GamesService.Core.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.GamesService.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GamesController : ControllerBase
	{
		private readonly IBggItemGetterService _bggItemGetterService;
		private readonly ICategoriesRepository _categoriesRepository;

		public GamesController(IBggItemGetterService bggItemGetterService, ICategoriesRepository categoriesRepository)
		{
			_bggItemGetterService = bggItemGetterService;
			_categoriesRepository = categoriesRepository;
		}

		/// <summary>
		/// Get items by BGG ids
		/// </summary>
		/// <param name="ids">Comma-separated list of BGG IDs (e.g., 123,456,789)</param>
		[HttpGet]
		public async Task<ActionResult<IEnumerable<BggItemResponse>>> GetItemsData([FromQuery] string ids)
		{
			if (string.IsNullOrWhiteSpace(ids))
				return BadRequest("No IDs provided.");

			var parsedIds = ids
				.Split(',', StringSplitOptions.RemoveEmptyEntries)
				.Select(s => int.TryParse(s, out var id) ? (int?)id : null)
				.Where(id => id.HasValue)
				.Select(id => id.Value)
				.ToList();

			if (!parsedIds.Any())
				return BadRequest("No valid IDs provided.");

			var items = await _bggItemGetterService.GetItemsByBggIds(parsedIds);

			if (!items.Any())
				return Ok(Array.Empty<BggItemResponse>());

			var allCategoryIds = items.SelectMany(i => i.BggMetadata.Categories ?? Enumerable.Empty<int>())
									  .Distinct()
									  .ToList();

			var allCategories = await _categoriesRepository.GetCategoriesByIdsAsync(allCategoryIds);
			var categoryDict = allCategories.ToDictionary(c => c.Id, c => c.Name ?? string.Empty);

			var responses = items.Select(item => new BggItemResponse
			{
				BggId = item.BggMetadata.BggId,
				Name = item.BggMetadata.Name,
				Categories = item.BggMetadata.Categories?
								.Select(id => categoryDict.TryGetValue(id, out var name) ? name : "Unknown")
								.ToList() ?? new List<string>()
			});

			return Ok(responses);
		}


	}
}
