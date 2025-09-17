using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Assemblers.DTOs
{
	public class ListingGroupDtoComposer
	{
		//private readonly IListingGroupGetterService _listingGroupGetterService;
		private readonly ILogger<ListingGroupDtoComposer> _logger;
		private readonly IListingGetterService _listingGetterService;
		private readonly IListingItemRepository _listingItemRepository;
		private readonly IListingQueueEntryRepository _listingQueueEntryRepository;

		public ListingGroupDtoComposer(ILogger<ListingGroupDtoComposer> logger, IListingGetterService listingGetterService, IListingItemRepository listingItemRepository, IListingQueueEntryRepository listingQueueEntryRepository)
		{
			_logger = logger;
			_listingGetterService = listingGetterService;
			_listingItemRepository = listingItemRepository;
			_listingQueueEntryRepository = listingQueueEntryRepository;
		}

		public async Task<ListingGroupResponseDto> ComposeListingGroupAsync(ListingGroup listingGroup)
		{
			IEnumerable<Listing> listings = await _listingGetterService.GetListingsByListingGroupIdAsync(listingGroup.Id);
			IEnumerable<Guid> listingIds = listings.Select(l => l.Id);

			Task<IEnumerable<ListingItem>> itemsTask = _listingItemRepository.GetListingItemsByListingIdsAsync(listingIds);
			Task<IEnumerable<ListingQueueEntry>> queuesTask = _listingQueueEntryRepository.GetListingQueueEntriesByListingIdsAsync(listingIds);

			await Task.WhenAll(itemsTask, queuesTask);

			IEnumerable<ListingItem> allItems = await itemsTask;
			IEnumerable<ListingQueueEntry> allQueueEntries = await queuesTask;

			ILookup<Guid,ListingItem> itemsLookup = allItems.ToLookup(i => i.ListingId);
			ILookup<Guid, ListingQueueEntry> queuesLookup = allQueueEntries.ToLookup(qe => qe.ListingId);

			IEnumerable<ListingResponseDto> listingDtos = listings.Select(l => 
			{
				ListingResponseDto dto = l.ToDto();
				dto.ListingItems = itemsLookup[dto.Id].Select(i => i.ToDto());
				dto.QueueEntries = queuesLookup[dto.Id].Select(qe => qe.ToDto());
				return dto;
			});
			ListingGroupResponseDto listingGroupResponseDto = listingGroup.ToDto();
			listingGroupResponseDto.Listings = listingDtos;

			return listingGroupResponseDto;
		}
	}
}
