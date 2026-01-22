using BoardGameMarket.ListingsService.Core.Application.Commands;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Enums;
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
	}
}
