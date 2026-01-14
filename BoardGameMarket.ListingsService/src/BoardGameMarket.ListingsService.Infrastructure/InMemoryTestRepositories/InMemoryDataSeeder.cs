using AutoFixture;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.InMemoryTestRepositories
{
	public class InMemoryDataSeeder
	{
		private readonly IListingGroupRepository _listingGroupRepository;
		private readonly IListingRepository _listingRepository;
		private readonly IListingItemRepository _listingItemRepository;
		private readonly IListingQueueEntryRepository _listingQueueEntryRepository;
		private readonly IListingSoldRepository _listingSoldRepository;
		private readonly IFixture _fixture;
		private readonly JsonSerializerOptions _jsonOptions;
		private readonly string _seedPath;

		public InMemoryDataSeeder(IListingGroupRepository listingGroupRepository, IListingRepository listingRepository, IListingItemRepository listingItemRepository, IListingQueueEntryRepository listingQueueEntryRepository, IListingSoldRepository listingSoldRepository, JsonSerializerOptions jsonOptions, string seedPath)
		{
			_listingGroupRepository = listingGroupRepository;
			_listingRepository = listingRepository;
			_listingItemRepository = listingItemRepository;
			_listingQueueEntryRepository = listingQueueEntryRepository;
			_listingSoldRepository = listingSoldRepository;
			_fixture = new Fixture();
			_jsonOptions = jsonOptions;
			_seedPath = seedPath;
		}

		public async Task SeedAsync()
		{
			Task groupsTask = SeedListingGroups();
			Task listingsTask = SeedListings();
			Task itemsTask = SeedListingItems();

			await Task.WhenAll(groupsTask, listingsTask, itemsTask);
		}

		private async Task SeedListingGroups()
		{
			string json = File.ReadAllText(Path.Combine(_seedPath, "LISTING_GROUPS_MOCK_DATA.json"));

			IEnumerable<ListingGroup>? listingGroups = JsonSerializer.Deserialize<IEnumerable<ListingGroup>>(json, _jsonOptions);

			if (listingGroups == null)
			{
				return;
			}
			foreach(ListingGroup listingGroup in listingGroups)
			{
				await _listingGroupRepository.AddListingGroupAsync(listingGroup);
			}
		}

		private async Task SeedListings()
		{
			string json = File.ReadAllText(Path.Combine(_seedPath, "LISTINGS_MOCK_DATA.json"));

			IEnumerable<Listing>? listings = JsonSerializer.Deserialize<IEnumerable<Listing>>(json, _jsonOptions);

			if (listings == null)
			{
				return;
			}
			foreach (Listing listing in listings)
			{
				await _listingRepository.AddListingAsync(listing);
			}
		}

		private async Task SeedListingItems()
		{
			string json = File.ReadAllText(Path.Combine(_seedPath, "LISTING_ITEMS_MOCK_DATA.json"));

			IEnumerable<ListingItem>? listingItems = JsonSerializer.Deserialize<IEnumerable<ListingItem>>(json, _jsonOptions);

			if (listingItems == null)
			{
				return;
			}
			foreach (ListingItem listingItem  in listingItems)
			{
				await _listingItemRepository.AddListingItemAsync(listingItem);
			}
		}
	}
}
