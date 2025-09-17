using BoardGameMarket.BggCatalogService.Core.Domain.Entities;
using BoardGameMarket.BggCatalogService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.BggCatalogService.Infrastructure.Repositories
{
	internal class CategoriesInMemoryRepository : ICategoriesRepository
	{
		private readonly HashSet<Category> _categories;

		public CategoriesInMemoryRepository()
		{
			_categories = new HashSet<Category>();
		}

		public async Task<Category?> AddCategoryAsync(Category category)
		{
			_categories.Add(category);
			return category;
		}

		public async Task<bool> ExistsAsync(int id)
		{
			return _categories.Select(c => c.Id).Contains(id);
		}

		public async Task<IEnumerable<Category>> GetCategoriesByIdsAsync(IEnumerable<int> ids)
		{
			return _categories.Where(c => ids.Contains(c.Id));
		}

		public async Task<Category?> GetCategoryByIdAsync(int id)
		{
			return _categories.FirstOrDefault(c => c.Id == id);
		}
	}
}
