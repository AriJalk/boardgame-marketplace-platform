using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BoardGameMarket.ListingsService.Infrastructure.Seeders
{
	internal class ListingItemsSeeder : ISeeder
	{
		private readonly IListingItemRepository _repository;
		private readonly JsonSerializerOptions _jsonOptions;

		public ListingItemsSeeder(IListingItemRepository repository, JsonSerializerOptions jsonOptions)
		{
			_repository = repository;
			_jsonOptions = jsonOptions;
		}
		public async Task SeedAsync(string seedPath)
		{
			string json = File.ReadAllText(Path.Combine(seedPath, "LISTING_ITEMS_MOCK_DATA.json"));
			IEnumerable<ListingItem>? listingItems = JsonSerializer.Deserialize<IEnumerable<ListingItem>>(json, _jsonOptions);

			if (listingItems == null)
			{
				return;
			}
			await _repository.AddListingItemsAsync(listingItems);
		}
	}
}
