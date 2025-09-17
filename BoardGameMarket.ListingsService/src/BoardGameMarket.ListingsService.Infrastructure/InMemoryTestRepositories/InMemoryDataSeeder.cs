using AutoFixture;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Infrastructure.InMemoryTestRepositories
{
	public class InMemoryDataSeeder
	{
		private readonly IListingGroupRepository _listingGroupRepository;
		private readonly IListingRepository _listingRepository;
		private readonly IListingItemRepository _listingItemRepository;
		private readonly IListingQueueEntryRepository _listingQueueEntryRepository;
		private readonly IListingQueuePriorityEntryRepository _listingQueuePriorityEntryRepository;
		private readonly IListingSoldRepository _listingSoldRepository;
		private readonly IFixture _fixture;

		public InMemoryDataSeeder(IListingGroupRepository listingGroupRepository, IListingRepository listingRepository, IListingItemRepository listingItemRepository, IListingQueueEntryRepository listingQueueEntryRepository, IListingQueuePriorityEntryRepository listingQueuePriorityEntryRepository, IListingSoldRepository listingSoldRepository)
		{
			_listingGroupRepository = listingGroupRepository;
			_listingRepository = listingRepository;
			_listingItemRepository = listingItemRepository;
			_listingQueueEntryRepository = listingQueueEntryRepository;
			_listingQueuePriorityEntryRepository = listingQueuePriorityEntryRepository;
			_listingSoldRepository = listingSoldRepository;
			_fixture = new Fixture();
		}

		public async Task SeedAsync()
		{
			for (int i = 0; i < 100; i++)
			{
				ListingGroup listingGroup = _fixture.Create<ListingGroup>();
				await _listingGroupRepository.AddListingGroupAsync(listingGroup);
				for (int j = 0; j < 5; j++)
				{
					Listing listing = _fixture.Create<Listing>();
					listing.ListingGroupId = listingGroup.Id;
					await _listingRepository.AddListingAsync(listing);
					for (int k = 0; k < 10; k++)
					{
						ListingItem listingItem = _fixture.Create<ListingItem>();
						listingItem.ListingId = listing.Id;
						await _listingItemRepository.AddListingItemAsync(listingItem);
					}
					for (int k = 0; k < 10; k++)
					{
						ListingQueueEntry listingQueueEntry = _fixture.Create<ListingQueueEntry>();
						listingQueueEntry.ListingId = listing.Id;
						await _listingQueueEntryRepository.AddListingQueueEntryAsync(listingQueueEntry);
						if (k % 3 == 0)
						{
							ListingQueuePriorityEntry listingQueuePriorityEntry = new ListingQueuePriorityEntry() 
							{
								ListingQueueEntryId = listingQueueEntry.Id,
								Priority = k * 10 
							};
							await _listingQueuePriorityEntryRepository.AddListingQueuePriorityEntryAsync(listingQueuePriorityEntry);
						}
					}
				}
			}
		}
	}
}
