using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueuePriorityEntries;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Assemblers.DTOs
{
	internal class ListingQueueEntryDtoComposer
	{
		private readonly ILogger<ListingQueueEntryDtoComposer> _logger;
		private readonly IListingQueueEntryGetterService _listingQueueEntryGetterService;
		private readonly IListingQueuePriorityEntryGetterService _listingQueuePriorityEntryGetterService;

		public async Task<ListingQueueEntryResponseDto?> ComposeListingQueueEntryByIdAsync(Guid queueEntryId)
		{
			ListingQueueEntry? queueEntry = await _listingQueueEntryGetterService.GetListingQueueEntryByIdAsync(queueEntryId);
			ArgumentNullException.ThrowIfNull(queueEntry, nameof(queueEntry));

			ListingQueuePriorityEntry? priorityEntry = await _listingQueuePriorityEntryGetterService.GetListingQueuePriorityEntryAsync(queueEntryId);
			ListingQueueEntryResponseDto responseDto = queueEntry.ToDto();
			responseDto.PriorityWeight = priorityEntry?.Priority;
			return responseDto;
		}

		public async Task<IEnumerable<ListingQueueEntryResponseDto>> ComposeListingQueueEntriesByListingIdAsync(Guid listingId)
		{
			IEnumerable<ListingQueueEntry> queueEntries = await _listingQueueEntryGetterService.GetListingQueueEntriesByListingIdAsync(listingId);

			IEnumerable<ListingQueuePriorityEntry> priorityEntries = await _listingQueuePriorityEntryGetterService.GetListingQueuePriorityEntriesAsync(queueEntries.Select(e => e.Id));

			IEnumerable<ListingQueueEntryResponseDto> responseDtos = queueEntries.Select(e => e.ToDto());

			Dictionary<Guid, ListingQueueEntryResponseDto> dtoDict = queueEntries.ToDictionary(e => e.Id, e => e.ToDto());

			foreach (var priorityEntry in priorityEntries)
			{
				if (dtoDict.TryGetValue(priorityEntry.ListingQueueEntryId, out var dto))
				{
					dto.PriorityWeight = priorityEntry.Priority;
				}
			}

			return dtoDict.Values;
		}
	}
}
