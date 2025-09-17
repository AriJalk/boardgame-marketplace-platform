using BoardGameMarket.BggCatalogService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.BggCatalogService.Core.Application.ServiceContracts
{
	public interface ICategoryGetterService
	{
		Task<Dictionary<int, string>> GetCategoriesByBggItemsAsync(IEnumerable<BggItem> bggItems);
	}
}
