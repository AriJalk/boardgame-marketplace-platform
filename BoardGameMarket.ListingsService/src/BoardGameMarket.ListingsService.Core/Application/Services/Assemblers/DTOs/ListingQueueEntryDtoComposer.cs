using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Services;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Assemblers.DTOs
{
	public class ListingQueueEntryDtoComposer
	{
		private readonly ILogger<ListingQueueEntryDtoComposer> _logger;
		private readonly IListingQueueEntryGetterService _listingQueueEntryGetterService;
		private readonly QueueDomainService _queueDomainService;

		public ListingQueueEntryDtoComposer(ILogger<ListingQueueEntryDtoComposer> logger, IListingQueueEntryGetterService listingQueueEntryGetterService, QueueDomainService queueDomainService)
		{
			_logger = logger;
			_listingQueueEntryGetterService = listingQueueEntryGetterService;
			_queueDomainService = queueDomainService;
		}

		public async Task<ListingQueueEntryResponseDto?> ComposeListingQueueEntryByIdAsync(Guid queueEntryId)
		{
			ListingQueueEntry? queueEntry = await _listingQueueEntryGetterService.GetListingQueueEntryByIdAsync(queueEntryId);
			ArgumentNullException.ThrowIfNull(queueEntry, nameof(queueEntry));
			ListingQueueEntryResponseDto responseDto = queueEntry.ToDto();
			return responseDto;
		}

		public async Task<IEnumerable<ListingQueueEntryResponseDto>> ComposeListingQueueEntriesByListingIdAsync(Guid listingId)
		{
			IEnumerable<ListingQueueEntry> queueEntries = _queueDomainService.SortQueue(await _listingQueueEntryGetterService.GetListingQueueEntriesByListingIdAsync(listingId));

			return queueEntries.Select(e => e.ToDto());
		}
	}
}
