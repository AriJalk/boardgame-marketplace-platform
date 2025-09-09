using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.External.ListingItems
{
	public class AddListingItemsUseCase
	{
		private readonly IListingItemAdderService _listingItemAdderService;
		private readonly ILogger<AddListingItemsUseCase> _logger;

		public AddListingItemsUseCase(IListingItemAdderService listingItemAdderService, ILogger<AddListingItemsUseCase> logger)
		{
			_listingItemAdderService = listingItemAdderService;
			_logger = logger;
		}

		public async Task<IEnumerable<ListingItemResponseDto>> ExecuteAsync(IEnumerable<ListingItemAddRequest> listingItemAddRequests)
		{
			IEnumerable<ListingItem> listingItems = await _listingItemAdderService.AddListingItemsAsync(listingItemAddRequests);
			return listingItems.Select(i => i.ToDto());
		}
	}
}
