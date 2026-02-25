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

        public async Task AddListingQueueEntryAsync(ListingQueueEntry listingQueueEntry)
        {
            int rows = await _listingQueueEntryRepository.AddListingQueueEntryAsync(listingQueueEntry);
            if (rows == 0)
            {
                throw new InvalidOperationException();
            }
        }
        public async Task DeleteListingQueueEntriesByListingIdAsync(Guid listingId)
        {
            await _listingQueueEntryRepository.DeleteListingQueueEntiresByListingIdAsync(listingId);
        }

        public async Task DeleteListingQueueEntryAsync(Guid listingQueueEntryId)
        {
            await _listingQueueEntryRepository.DeleteListingQueueEntryAsync(listingQueueEntryId);
        }


        public async Task UpdateListingQueueEntryAsync(ListingQueueEntry listingQueueEntryToUpdate)
        {
            int rows = await _listingQueueEntryRepository.UpdateListingQueueEntryAsync(listingQueueEntryToUpdate);
            if (rows == 0)
            {
                throw new InvalidOperationException();
            }
        }


        public async Task UpdateListingQueueEntriesAsync(IEnumerable<ListingQueueEntry> listingQueueEntries)
        {
            List<ListingQueueEntry> queueEntries = listingQueueEntries.ToList();
            int rows = await _listingQueueEntryRepository.UpdateListingQueueEntriesAsync(queueEntries);
            if (rows != queueEntries.Count)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
