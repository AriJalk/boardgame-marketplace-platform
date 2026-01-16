using BoardGameMarket.CatalogService.Core.Application.Commands;
using BoardGameMarket.CatalogService.Core.Application.ServiceContracts;
using BoardGameMarket.CatalogService.Core.Domain.Entities;
using BoardGameMarket.CatalogService.Core.Domain.Enums;
using BoardGameMarket.CatalogService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.CatalogService.Core.Application.Services
{
	public class CatalogItemAdderService : ICatalogItemAdderService
	{
		private readonly ICatalogItemsRepository _repository;

		public CatalogItemAdderService(ICatalogItemsRepository repository)
		{
			_repository = repository;
		}

		public async Task<CatalogItem?> AddBggCatalogItemAsync(AddBggCatalogItemCommand addBggCatalogItemCommand)
		{
			CatalogItem catalogItemToAdd = new CatalogItem()
			{
				Id = Guid.NewGuid(),
				Type = CatalogItemType.Bgg,
				BggId = addBggCatalogItemCommand.Metadata.BggId,
				Name = addBggCatalogItemCommand.Metadata.Name,
				ModifiedTimestamp = DateTime.UtcNow,
			};
			return await _repository.AddCatalogItemToDatabaseAsync(catalogItemToAdd);
		}

		public async Task<CatalogItem?> AddCustomCatalogItemAsync(AddCustomCatalogItemCommand addCustomCatalogItemCommand)
		{
			CatalogItem catalogItemToAdd = new CatalogItem()
			{
				Id = Guid.NewGuid(),
				Type = CatalogItemType.Custom,
				Name = addCustomCatalogItemCommand.Name,
				ModifiedTimestamp = DateTime.UtcNow,
			};
			return await _repository.AddCatalogItemToDatabaseAsync(catalogItemToAdd);
		}
	}
}
