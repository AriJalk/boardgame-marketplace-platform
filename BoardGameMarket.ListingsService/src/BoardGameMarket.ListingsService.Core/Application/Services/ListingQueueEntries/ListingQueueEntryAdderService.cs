using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingQueueEntries
{
	internal class ListingQueueEntryAdderService : IListingQueueEntryAdderService
	{
		private readonly IListingQueueEntryRepository _listingQueueEntryRepository;

		public ListingQueueEntryAdderService(IListingQueueEntryRepository listingQueueEntryRepository)
		{
			_listingQueueEntryRepository = listingQueueEntryRepository;
		}

		public async Task<ListingQueueEntry?> AddListingQueueEntryAsync(ListingQueueEntry listingQueueEntryToAdd)
		{
			listingQueueEntryToAdd.Id = Guid.NewGuid();
			listingQueueEntryToAdd.CreatedAt = DateTime.UtcNow;
			listingQueueEntryToAdd.ModifiedAt = DateTime.UtcNow;

			return await _listingQueueEntryRepository.AddListingQueueEntryAsync(listingQueueEntryToAdd);
		}
	}
}
