using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Composition;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.ListingItems;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingItems;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.External.ListingItems
{
	internal class AddCompleteItemsUseCase
	{
		private readonly IListingItemAdderService _listingItemAdderService;
		private readonly ILogger<AddCompleteItemsUseCase> _logger;

		public AddCompleteItemsUseCase(IListingItemAdderService listingItemAdderService, ILogger<AddCompleteItemsUseCase> logger)
		{
			_listingItemAdderService = listingItemAdderService;
			_logger = logger;
		}

		private void ValidateListingConsistency(IEnumerable<ListingItemAddRequest> requests)
		{
			Guid listingId = requests.First().ListingId;
			if (requests.Any(r => r.ListingId != listingId))
			{
				throw new InvalidOperationException("All items must belong to the same listing");
			}
		}

		public async Task<IEnumerable<ListingItemResponseDto>> ExecuteAsync(IEnumerable<ListingItemAddRequest> listingItemAddRequests)
		{
			ValidateListingConsistency(listingItemAddRequests);
			IEnumerable<ListingItem> listingItems = await _listingItemAdderService.AddListingItemsAsync(listingItemAddRequests.Select(i => i.ToListingItem()));
			return listingItems.Select(i => i.ToDto());
		}
	}
}
