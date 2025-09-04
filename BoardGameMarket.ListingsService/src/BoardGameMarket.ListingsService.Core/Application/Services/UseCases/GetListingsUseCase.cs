using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGames;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingQueueEntries;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
namespace BoardGameMarket.ListingsService.Core.Application.Services.UseCases
{
	public class GetListingsUseCase
	{
		private readonly IListingGetterService _listingGetterService;
		private readonly IListingGameGetterService _listingGameGetterService;
		private readonly IListingQueueEntriesGetterService _listingQueueEntriesGetterService;

		private async Task AttachListingGamesToListingDtoAsync(ListingResponseDto listingDto)
		{
			IEnumerable<ListingGame> listingGames = await _listingGameGetterService.GetListingGamesByListingIdAsync(listingDto.Id);

			listingDto.ListingGames = listingGames.Select(g => g.ToDto());
		}
		private async Task AttachListingQueueEntriesToListingDtoAsync(ListingResponseDto listingDto)
		{
			IEnumerable<ListingQueueEntry> listingQueueEntries = await _listingQueueEntriesGetterService.GetListingQueueEntriesByListingIdAsync(listingDto.Id);

			listingDto.QueueEntries = listingQueueEntries.Select(e => e.ToDto());
		}

		private async Task<ListingResponseDto> BuildListingDtoAsync(Listing listing)
		{
			ListingResponseDto listingDto = listing.ToDto();
			await AttachListingGamesToListingDtoAsync(listingDto);
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
