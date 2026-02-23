using BoardGameMarket.CatalogService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.CatalogService.Core.Application.ServiceContracts
{
	public interface ICatalogItemGetterService
	{
		Task<CatalogItem?> GetCatalogItemByInternalIdASync(Guid internalId);
		Task<CatalogItem?> GetBggItemByBggIdAsync(int bggId);
		Task<CatalogItem?> GetCustomItemByNameAsync(string name);
		Task<IEnumerable<CatalogItem>> GetCustomItemsByNamesAsync(IEnumerable<string> names);
		Task<IEnumerable<CatalogItem>> GetCatalogItemsAsync();
	}
}
