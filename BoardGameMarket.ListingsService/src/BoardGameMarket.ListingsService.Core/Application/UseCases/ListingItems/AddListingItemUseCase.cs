using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingItems
{
	public class AddListingItemUseCase
	{
		private readonly ILogger<AddListingItemUseCase> _logger;
		private readonly IListingItemAdderService _listingItemAdderService;
		private readonly ListingFetcher _listingFetcher;

		public AddListingItemUseCase(ILogger<AddListingItemUseCase> logger, IListingItemAdderService listingItemAdderService, ListingFetcher listingFetcher)
		{
			_logger = logger;
			_listingItemAdderService = listingItemAdderService;
			_listingFetcher = listingFetcher;
		}

		public async Task<ListingItemResponseDto> ExecuteAsync(ListingItemAddRequest listingItemAddRequest)
		{

			if (listingItemAddRequest.ListingId == Guid.Empty)
			{
				throw new ArgumentException("ListingId empty");
			}

			// Validate existance of listing
			await _listingFetcher.GetListingByIdAsync(listingItemAddRequest.ListingId);

			ListingItem? listingItem = await _listingItemAdderService.AddListingItemAsync(listingItemAddRequest.ToListingItem());
			if (listingItem == null)
			{
				throw new InvalidOperationException("Can't create listing item");
			}
			return listingItem.ToDto();
		}
	}
}
