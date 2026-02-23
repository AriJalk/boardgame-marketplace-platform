using BoardGameMarket.CatalogService.Core.Application.ServiceContracts;
using BoardGameMarket.CatalogService.Core.Domain.Entities;
using BoardGameMarket.CatalogService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.CatalogService.Core.Application.Services
{
	public class CatalogItemGetterService : ICatalogItemGetterService
	{
		private readonly ICatalogItemsRepository _repository;

		public CatalogItemGetterService(ICatalogItemsRepository repository)
		{
			_repository = repository;
		}

		public async Task<CatalogItem?> GetBggItemByBggIdAsync(int bggId)
		{
			return await _repository.GetBggItemByBggIdAsync(bggId);
		}

		public async Task<CatalogItem?> GetCatalogItemByInternalIdASync(Guid internalId)
		{
			return await _repository.GetItemByDatabaseIdAsync(internalId);
		}

		public Task<IEnumerable<CatalogItem>> GetCatalogItemsAsync()
		{
			return _repository.GetCatalogItemsAsync();
		}

		public async Task<CatalogItem?> GetCustomItemByNameAsync(string name)
		{
			return await _repository.GetCustomItemByNameAsync(name);
		}

        public async Task<IEnumerable<CatalogItem>> GetCustomItemsByNamesAsync(IEnumerable<string> names)
        {
			return await _repository.GetCustomItemsByNamesAsync(names);
        }
    }
}
