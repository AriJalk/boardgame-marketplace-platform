using BoardGameMarket.BggCatalogService.Core.Application.ServiceContracts;
using BoardGameMarket.BggCatalogService.Core.Domain.Entities;
using BoardGameMarket.BggCatalogService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.BggCatalogService.Core.Application.Services
{
	internal class CategoryGetterService : ICategoryGetterService
	{
		private readonly ICategoriesRepository _categoriesRepository;
		public async Task<Dictionary<int, string>> GetCategoriesByBggItemsAsync(IEnumerable<BggItem> bggItems)
		{
			IEnumerable<int> allCategoryIds = bggItems.SelectMany(i => i.BggMetadata.Categories ?? Enumerable.Empty<int>())
									  .Distinct()
									  .ToList();

			IEnumerable<Category> allCategories = await _categoriesRepository.GetCategoriesByIdsAsync(allCategoryIds);
			return allCategories.ToDictionary(c => c.Id, c => c.Name ?? string.Empty);
		}
	}
}
