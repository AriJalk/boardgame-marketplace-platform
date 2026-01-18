using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using BoardGameMarket.ListingsService.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BoardGameMarket.ListingsService.Infrastructure.InMemoryRepositories.Seeders
{
	public class ListingGroupsSeeder : ISeeder
	{
		private readonly IListingGroupRepository _repository;
		private readonly JsonSerializerOptions _jsonOptions;

		public ListingGroupsSeeder(IListingGroupRepository repository, JsonSerializerOptions jsonOptions)
		{
			_repository = repository;
			_jsonOptions = jsonOptions;
		}
		public async Task SeedAsync(string seedPath)
		{
			string json = File.ReadAllText(Path.Combine(seedPath, "Listings", "LISTING_GROUPS_MOCK_DATA.json"));

			IEnumerable<ListingGroup>? listingGroups = JsonSerializer.Deserialize<IEnumerable<ListingGroup>>(json, _jsonOptions);

			if (listingGroups == null)
			{
				return;
			}
			foreach (ListingGroup listingGroup in listingGroups)
			{
				await _repository.AddListingGroupAsync(listingGroup);
			}
		}
	}
}
