using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingQueueEntries
{
	internal class SetQueueEntryPriorityUseCase
	{
		private readonly ListingFetcher _listingFetcher;
		private readonly IListingQueueEntryUpdaterService _listingQueueEntryUpdaterService;

		public SetQueueEntryPriorityUseCase(ListingFetcher listingFetcher, IListingQueueEntryUpdaterService listingQueueEntryUpdaterService)
		{
			_listingFetcher = listingFetcher;
			_listingQueueEntryUpdaterService = listingQueueEntryUpdaterService;
		}

		public async Task<ListingQueueEntryResponseDto> ExecuteAsync(ListingQueueEntrySetPriorityRequest listingQueueEntrySetPriorityRequest)
		{
			ListingQueueEntry listingQueueEntry = await _listingFetcher.GetListingQueueEntryByIdAsync(listingQueueEntrySetPriorityRequest.ListingQueueEntryId);
			Guid sellerId = await _listingFetcher.GetSellerUserIdByListingIdAsync(listingQueueEntry.ListingId);
			listingQueueEntry.Priority = listingQueueEntrySetPriorityRequest.Priority;
			ListingQueueEntry? listingQueueEntryFromUpdate = await _listingQueueEntryUpdaterService.UpdateListingQueueEntryAsync(listingQueueEntry);
			if (listingQueueEntryFromUpdate == null)
			{
				throw new InvalidOperationException("Can't update queue entry");
			}

			return listingQueueEntryFromUpdate.ToDto();
		}
	}
}
