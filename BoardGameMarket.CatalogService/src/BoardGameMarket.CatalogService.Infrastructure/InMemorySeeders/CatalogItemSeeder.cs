using BoardGameMarket.CatalogService.Core.Domain.Entities;
using BoardGameMarket.CatalogService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BoardGameMarket.CatalogService.Infrastructure.InMemorySeeders
{
	internal class CatalogItemSeeder : ISeeder
	{
		private readonly ICatalogItemsRepository _repository;
		private readonly JsonSerializerOptions _jsonOptions;

		public CatalogItemSeeder(ICatalogItemsRepository repository, JsonSerializerOptions jsonOptions)
		{
			_repository = repository;
			_jsonOptions = jsonOptions;
		}

		public async Task SeedAsync(string seedPath)
		{
			string json = File.ReadAllText(Path.Combine(seedPath, "CATALOG_ITEMS_MOCK_DATA.json"));

			IEnumerable<CatalogItem>? catalogItems = JsonSerializer.Deserialize<IEnumerable<CatalogItem>>(json, _jsonOptions);

			if (catalogItems == null)
			{
				return;
			}
			foreach (CatalogItem catalogItem in catalogItems)
			{
				await _repository.AddCatalogItemToDatabaseAsync(catalogItem);
			}
		}
	}
}
