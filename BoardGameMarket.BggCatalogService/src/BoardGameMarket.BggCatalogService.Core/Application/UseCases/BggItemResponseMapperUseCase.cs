using BoardGameMarket.BggCatalogService.Core.Application.DTOs;
using BoardGameMarket.BggCatalogService.Core.Application.ServiceContracts;
using BoardGameMarket.BggCatalogService.Core.Application.Services;
using BoardGameMarket.BggCatalogService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.BggCatalogService.Core.Application.UseCases
{
	public class BggItemResponseMapperUseCase
	{

		private ICategoryGetterService _categoryGetterService;

		public async Task<IEnumerable<BggItemResponse>> MapToResponseAsync(IEnumerable<BggItem> items)
		{
			if (!items.Any())
			{
				return Array.Empty<BggItemResponse>();
			}

			Dictionary<int, string> categoryDict = await _categoryGetterService.GetCategoriesByBggItemsAsync(items);

			IEnumerable<BggItemResponse> responses = items.Select(item => new BggItemResponse
			{
				BggId = item.BggMetadata.BggId,
				Name = item.BggMetadata.Name,
				Categories = item.BggMetadata.Categories?
								.Select(id => categoryDict.TryGetValue(id, out var name) ? name : "Unknown")
								.ToList() ?? new List<string>()
			});
			return responses;
		}
	}
}
