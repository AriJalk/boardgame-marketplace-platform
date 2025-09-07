using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.Listings
{
	public class GetListingsUseCase
	{
		private readonly IListingGetterService _listingGetterService;
		private readonly IListingItemGetterService _listingItemGetterService;
		private readonly IListingQueueEntryGetterService _listingQueueEntryGetterService;

		private async Task AttachListingItemsToListingDtoAsync(ListingResponseDto listingDto)
		{
			IEnumerable<ListingItem> listingItems = await _listingItemGetterService.GetListingItemsByListingIdAsync(listingDto.Id);

			listingDto.ListingItems = listingItems.Select(g => g.ToDto());
		}
		private async Task AttachListingQueueEntriesToListingDtoAsync(ListingResponseDto listingDto)
		{
			IEnumerable<ListingQueueEntry>? listingQueueEntries = await _listingQueueEntryGetterService.GetListingQueueEntriesByListingIdAsync(listingDto.Id);
			ArgumentNullException.ThrowIfNull(listingQueueEntries, nameof(listingQueueEntries));

			listingDto.QueueEntries = listingQueueEntries.Select(e => e.ToDto());
		}

		private async Task<ListingResponseDto> BuildListingDtoAsync(Listing listing)
		{
			ListingResponseDto listingDto = listing.ToDto();
			await AttachListingItemsToListingDtoAsync(listingDto);
			await AttachListingQueueEntriesToListingDtoAsync(listingDto);

			return listingDto;
		}

		public async Task<ListingResponseDto> ExecuteByListingIdAsync(Guid listingId)
		{
			Listing? listing = await _listingGetterService.GetListingByIdAsync(listingId);
			if (listing == null)
			{
				throw new KeyNotFoundException($"Listing {listingId} not found");
			}

			ListingResponseDto listingDto = await BuildListingDtoAsync(listing);
			listingDto.ListingGroupId = listing.ListingGroupId;

			return listingDto;
		}

		public async Task<IEnumerable<ListingResponseDto>> ExecuteByListingGroupIdAsync(Guid listingGroupId)
		{
			IEnumerable<Listing> listings = await _listingGetterService.GetListingsByListingGroupIdAsync(listingGroupId);

			IEnumerable<Task<ListingResponseDto>> tasks = listings.Select(BuildListingDtoAsync);
			List<ListingResponseDto> listingDtos = (await Task.WhenAll(tasks)).ToList();

			return listingDtos;
		}
	}
}
