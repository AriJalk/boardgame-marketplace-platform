using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BoardGameMarket.ListingsService.Infrastructure.InMemoryTestRepositories.Seeders
{
	public class ListingsSeeder : ISeeder
	{
		private readonly IListingRepository _repository;
		private readonly JsonSerializerOptions _jsonOptions;

		public ListingsSeeder(IListingRepository repository, JsonSerializerOptions jsonOptions)
		{
			_repository = repository;
			_jsonOptions = jsonOptions;
		}
		public async Task SeedAsync(string seedPath)
		{
			string json = File.ReadAllText(Path.Combine(seedPath, "LISTINGS_MOCK_DATA.json"));

			IEnumerable<Listing>? listings = JsonSerializer.Deserialize<IEnumerable<Listing>>(json, _jsonOptions);

			if (listings == null)
			{
				return;
			}

			await _repository.AddListingsAsync(listings);
		}
	}
}
