using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace BoardGameMarket.ListingsService.Infrastructure.Seeders
{
	public class ListingsSeeder : ISeeder
	{
		private readonly IListingRepository _listingRepository;
		private readonly IListingGroupRepository _listingGroupRepository;
		private readonly JsonSerializerOptions _jsonOptions;

        public ListingsSeeder(IListingRepository listingRepository, IListingGroupRepository listingGroupRepository, JsonSerializerOptions jsonOptions)
        {
            _listingRepository = listingRepository;
            _listingGroupRepository = listingGroupRepository;
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

			Dictionary<Guid, ListingGroup> listingGroups = new Dictionary<Guid, ListingGroup>();

			foreach(Listing listing in listings)
			{
				if (!listingGroups.TryGetValue(listing.ListingGroupId, out var existingListingGroup))
				{
					ListingGroup? listingGroupFromRepo = await _listingGroupRepository.GetListingGroupByIdAsync(listing.ListingGroupId);
					if (listingGroupFromRepo == null)
					{
						throw new InvalidOperationException();
					}
					listingGroups[listing.ListingGroupId] = listingGroupFromRepo;
				}
				listing.SellerUserId = listingGroups[listing.ListingGroupId].SellerUserId;
			}

			await _listingRepository.AddListingsAsync(listings);
		}
	}
}
