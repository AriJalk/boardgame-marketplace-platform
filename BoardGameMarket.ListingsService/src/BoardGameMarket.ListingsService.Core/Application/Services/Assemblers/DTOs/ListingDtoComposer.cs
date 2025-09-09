using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Assemblers.DTOs
{
	internal class ListingDtoComposer
	{
		private readonly IListingGetterService _listingGetterService;
		private readonly ListingItemDtoComposer _listingItemDtosComposer;
		private readonly ListingQueueEntryDtoComposer _listingQueueEntryDtosComposer;

		private async Task<ListingResponseDto> ComposeCompleteListingAsync (Listing listing)
		{
			ListingResponseDto listingDto = listing.ToDto();

			Task<IEnumerable<ListingItemResponseDto>> itemsTask = _listingItemDtosComposer.ComposeListingItemsAsync(listingDto.Id);
			Task<IEnumerable<ListingQueueEntryResponseDto>> queueTasks = _listingQueueEntryDtosComposer.ComposeListingQueueEntriesByListingIdAsync(listingDto.Id);
			await Task.WhenAll(itemsTask, queueTasks);

			listingDto.ListingItems = await itemsTask;
			listingDto.QueueEntries = await queueTasks;

			return listingDto;
		}

		public async Task<ListingResponseDto> ComposeByIdAsync(Guid listingId)
		{
			Listing? listing = await _listingGetterService.GetListingByIdAsync(listingId);
			if (listing == null)
			{
				throw new KeyNotFoundException($"Listing {listingId} not found");
			}
			return await ComposeCompleteListingAsync(listing);
		}

		public async Task<IEnumerable<ListingResponseDto>> ComposeByGroupIdAsync(Guid listingGroupId)
		{
			IEnumerable<Listing> listings = await _listingGetterService.GetListingsByListingGroupIdAsync(listingGroupId);
			IEnumerable<Task<ListingResponseDto>> tasks =  listings.Select(ComposeCompleteListingAsync);

			return await Task.WhenAll(tasks);
		}
	}
}
