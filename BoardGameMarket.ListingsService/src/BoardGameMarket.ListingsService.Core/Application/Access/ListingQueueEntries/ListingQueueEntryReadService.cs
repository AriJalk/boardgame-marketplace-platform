using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.Access.ListingQueueEntries
{
	internal class ListingQueueEntryReadService : IListingQueueEntryReadService
	{
		private readonly IListingQueueEntryRepository _listingQueueEntryRepository;

		public ListingQueueEntryReadService(IListingQueueEntryRepository listingQueueEntryRepository)
		{
			_listingQueueEntryRepository = listingQueueEntryRepository;
		}


		public async Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesBySubscribedUserIdAsync(Guid buyerUserId)
		{
			return await _listingQueueEntryRepository.GetListingQueueEntriesBySubscribedUserId(buyerUserId);
		}

		public async Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByListingIdAsync(Guid listingId)
		{
			IEnumerable<ListingQueueEntry> queueEntries = await _listingQueueEntryRepository.GetListingQueueEntriesByListingIdAsync(listingId);

			return queueEntries.OrderBy(qe => qe.CreatedAt);
		}

		public async Task<IEnumerable<ListingQueueEntry>> GetListingQueueEntriesByListingIdsAsync(IEnumerable<Guid> listingIds)
		{
			IEnumerable<ListingQueueEntry> queueEntries = await _listingQueueEntryRepository.GetListingQueueEntriesByListingIdsAsync(listingIds);
			return queueEntries.OrderBy(qe => qe.CreatedAt);
		}

		public async Task<ListingQueueEntry?> GetListingQueueEntryByIdAsync(Guid listingQueueId)
		{
			return await _listingQueueEntryRepository.GetListingQueueEntryByIdAsync(listingQueueId);
		}

		public async Task<ListingQueueEntry?> GetListingQueueEntryByListingIdAndUserId(Guid listingId, Guid buyerId)
		{
			return await _listingQueueEntryRepository.GetListingQueueEntryByListingIdAndBuyerUserId(listingId, buyerId);
		}
	}
}
