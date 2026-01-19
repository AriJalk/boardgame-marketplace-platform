using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases
{
	public class GetQueueUseCase
	{
		private readonly IListingQueueEntryGetterService _listingQueueEntryGetterService;

		public GetQueueUseCase(IListingQueueEntryGetterService listingQueueEntryGetterService)
		{
			_listingQueueEntryGetterService = listingQueueEntryGetterService;
		}

		public async Task<ListingQueueInfo> Execute(Guid listingId)
		{
			IEnumerable<ListingQueueEntry> allEntries = await _listingQueueEntryGetterService.GetListingQueueEntriesByListingIdAsync(listingId);

			ListingQueueEntryInfo[] queueByTime = allEntries
				.OrderBy(qe => qe.CreatedAt)
				.Select(qe => qe.ToListingQueueEntryInfo())
				.ToArray();

			ListingQueueEntryInfo[] queueByPriority = allEntries
				.Where(qe => qe.Priority != null)
				.OrderBy(qe => qe.Priority)
				.Select(qe => qe.ToListingQueueEntryInfo())
				.ToArray();

			return new ListingQueueInfo(queueByTime, queueByPriority);
		}
	}
}
