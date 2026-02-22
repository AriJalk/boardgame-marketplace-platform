using BoardGameMarket.ListingsService.Core.Application.Commands;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries
{
    internal class ListingQueueEntryWriteService : IListingQueueEntryWriteService
	{
		private readonly IListingQueueEntryRepository _listingQueueEntryRepository;

		public ListingQueueEntryWriteService(IListingQueueEntryRepository listingQueueEntryRepository)
		{
			_listingQueueEntryRepository = listingQueueEntryRepository;
		}

		public async Task<ListingQueueEntry?> AddListingQueueEntryAsync(CreateListingQueueEntryCommand command)
		{
			ListingQueueEntry listingQueueEntry = new ListingQueueEntry()
			{
				Id = Guid.NewGuid(),
				BuyerUserId = command.UserId,
				ListingId = command.ListingId,
				CreatedAt = DateTime.UtcNow,
				ModifiedAt = DateTime.UtcNow,
				Status = QueueStatus.Queued,
				Priority = null,
			};

			return await _listingQueueEntryRepository.AddListingQueueEntryAsync(listingQueueEntry);
		}
		public async Task DeleteListingQueueEntriesByListingIdAsync(Guid listingId)
		{
			await _listingQueueEntryRepository.DeleteListingQueueEntiresByListingIdAsync(listingId);
		}

		public async Task<bool> DeleteListingQueueEntryAsync(Guid listingQueueEntryId)
		{
			return await _listingQueueEntryRepository.DeleteListingQueueEntryAsync(listingQueueEntryId);
		}


        public async Task<ListingQueueEntry?> UpdateListingQueueEntryAsync(ListingQueueEntry listingQueueEntryToUpdate)
		{
			listingQueueEntryToUpdate.ModifiedAt = DateTime.UtcNow;
			return await _listingQueueEntryRepository.UpdateListingQueueEntryAsync(listingQueueEntryToUpdate);
		}


        public async Task<IEnumerable<ListingQueueEntry>> UpdateListingQueueEntriesAsync(IEnumerable<ListingQueueEntry> listingQueueEntries)
        {
            return await _listingQueueEntryRepository.UpdateListingQueueEntriesAsync(listingQueueEntries);
        }
    }
}
