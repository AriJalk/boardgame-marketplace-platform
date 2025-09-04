using BoardGameMarket.GamesService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.GamesService.Core.Domain.RepositoryContracts
{
	public interface ICategoriesRepository
	{
		Task<Category?> GetCategoryByIdAsync(int id);
		Task<IEnumerable<Category>> GetCategoriesByIdsAsync(IEnumerable<int> ids);
		Task<Category?> AddCategoryAsync(Category category);
		Task<bool> ExistsAsync(int id);
	}
}
