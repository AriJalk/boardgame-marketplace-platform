using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingItems
{
	/// <summary>
	/// Used to bulk add listing items, trusts caller assigned ListingId.
	/// </summary>
	public class AddCompleteListingItemsUseCase
	{
		private readonly IListingItemAdderService _listingItemAdderService;
		private readonly ILogger<AddCompleteListingItemsUseCase> _logger;

		public AddCompleteListingItemsUseCase(IListingItemAdderService listingItemAdderService, ILogger<AddCompleteListingItemsUseCase> logger)
		{
			_listingItemAdderService = listingItemAdderService;
			_logger = logger;
		}

		public async Task<IEnumerable<ListingItemResponseDto>> ExecuteAsync(IEnumerable<ListingItemAddRequest> listingItemAddRequests)
		{
			IEnumerable<ListingItem> listingItems = await _listingItemAdderService.AddListingItemsAsync(listingItemAddRequests.Select(i => i.ToListingItem()));
			return listingItems.Select(i => i.ToDto());
		}
	}
}
