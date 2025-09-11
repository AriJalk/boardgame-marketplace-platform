using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Services.ListingQueueEntries
{
	internal class ListingQueueEntryGetterService : IListingQueueEntryGetterService
	{
		private readonly IListingQueueEntryRepository _listingQueueEntryRepository;
		private readonly ILogger<ListingQueueEntryGetterService> _logger;

		public ListingQueueEntryGetterService(IListingQueueEntryRepository listingQueueEntryRepository, ILogger<ListingQueueEntryGetterService> logger)
		{
			_listingQueueEntryRepository = listingQueueEntryRepository;
			_logger = logger;
		}

		public async Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntiresByBuyerUserIdAsync(Guid buyerUserId)
		{
			_logger.LogInformation($"{nameof(ListingQueueEntryGetterService)}:{nameof(GetListingQueueEntiresByBuyerUserIdAsync)}- Get listing queues of buyer {buyerUserId}");

			return await _listingQueueEntryRepository.GetListingQueueEntriesByBuyerUserId(buyerUserId);
		}

		public async Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByListingIdAsync(Guid listingId)
		{
			_logger.LogInformation($"{nameof(ListingQueueEntryGetterService)}:{nameof(GetListingQueueEntriesByListingIdAsync)}- Get listing queues by listing {listingId}");

			IEnumerable<ListingQueueEntry> queueEntries = await _listingQueueEntryRepository.GetListingQueueEntriesByListingIdAsync(listingId);

			return queueEntries.OrderBy(qe => qe.CreatedAt);
		}

		public async Task<ListingQueueEntry?> GetListingQueueEntryByIdAsync(Guid listingQueueId)
		{
			_logger.LogInformation($"{nameof(ListingQueueEntryGetterService)}:{nameof(GetListingQueueEntryByIdAsync)}- Get listing queue {listingQueueId}");

			return await _listingQueueEntryRepository.GetListingQueueEntryByIdAsync(listingQueueId);
		}
	}
}
