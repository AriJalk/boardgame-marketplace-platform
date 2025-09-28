using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using BoardGameMarket.ListingsService.Core.Domain.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Assemblers.DTOs
{
	public class ListingGroupDtoComposer
	{
		//private readonly IListingGroupGetterService _listingGroupGetterService;
		private readonly ILogger<ListingGroupDtoComposer> _logger;
		private readonly IListingGetterService _listingGetterService;
		private readonly IListingItemGetterService _listingItemGetterService;
		private readonly IListingQueueEntryGetterService _listingQueueEntryGetterService;
		private readonly RequestCache _requestCache;
		private readonly QueueDomainService _queueDomainService;

		public ListingGroupDtoComposer(ILogger<ListingGroupDtoComposer> logger, IListingGetterService listingGetterService, IListingItemGetterService listingItemGetterService, IListingQueueEntryGetterService listingQueueEntryGetterService, RequestCache requestCache, QueueDomainService queueDomainService)
		{
			_logger = logger;
			_listingGetterService = listingGetterService;
			_listingItemGetterService = listingItemGetterService;
			_listingQueueEntryGetterService = listingQueueEntryGetterService;
			_requestCache = requestCache;
			_queueDomainService = queueDomainService;
		}

		public async Task<ListingGroupResponseDto> ComposeListingGroupAsync(ListingGroup listingGroup)
		{
			IEnumerable<Listing> listings = await _listingGetterService.GetListingsByListingGroupIdAsync(listingGroup.Id);

			IEnumerable<Guid> listingIds = listings.Select(l => l.Id);

			Task<IEnumerable<ListingItem>> itemsTask = _listingItemGetterService.GetListingItemsByListingIdsAsync(listingIds);
			Task<IEnumerable<ListingQueueEntry>> queuesTask = _listingQueueEntryGetterService.GetListingQueueEntriesByListingIdsAsync(listingIds);

			await Task.WhenAll(itemsTask, queuesTask);

			//IEnumerable<ListingItem> allItems = await itemsTask;
			//IEnumerable<ListingQueueEntry> allQueueEntries = await queuesTask;

			//ILookup<Guid,ListingItem> itemsLookup = allItems.ToLookup(i => i.ListingId);
			//ILookup<Guid, ListingQueueEntry> queuesLookup = allQueueEntries.ToLookup(qe => qe.ListingId);


			ListingResponseDto[] listingDtos = listings.Select(l =>
			{
				ListingResponseDto dto = l.ToDto();
				dto.ListingItems = _requestCache.GetListingItemsByListingId(dto.Id).Select(i => i.ToDto()).ToArray();
				dto.QueueEntries = _queueDomainService.SortQueue(_requestCache.GetListingQueueEntriesByListingId(dto.Id)).Select(qe => qe.ToDto()).ToArray();
				return dto;
			}).ToArray();

			ListingGroupResponseDto listingGroupResponseDto = listingGroup.ToDto();
			listingGroupResponseDto.Listings = listingDtos;
			return listingGroupResponseDto;
		}
	}
}
