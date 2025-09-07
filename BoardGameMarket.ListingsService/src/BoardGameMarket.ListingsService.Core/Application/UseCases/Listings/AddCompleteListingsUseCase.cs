using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Composition;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingItems;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.Listings
{
	public class AddCompleteListingsUseCase
	{
		private readonly IListingAdderService _listingAdderService;
		private readonly AddListingItemsUseCase _addListingItemsUseCase;
		private readonly ILogger<AddCompleteListingsUseCase> _logger;

		public AddCompleteListingsUseCase(IListingAdderService listingAdderService, AddListingItemsUseCase addListingItemsUseCase, ILogger<AddCompleteListingsUseCase> logger)
		{
			_listingAdderService = listingAdderService;
			_addListingItemsUseCase = addListingItemsUseCase;
			_logger = logger;
		}

		private async Task<ListingResponseDto> BuildListingWithItems(ListingWithListingItemsRequest request)
		{
			var listing = await _listingAdderService.AddListingAsync(request.ListingAddRequest);
			if (listing == null)
			{
				_logger.LogError($"{nameof(AddCompleteListingsUseCase)}:{nameof(BuildListingWithItems)} - can't add listing");
				throw new InvalidOperationException("Failed to create listing");
			}

			foreach (var itemRequest in request.ListingItemAddRequests)
			{
				itemRequest.ListingId = listing.Id;
			}

			var itemResponses = await _addListingItemsUseCase.ExecuteAsync(request.ListingItemAddRequests);

			var response = listing.ToDto();
			response.ListingItems = itemResponses;
			return response;
		}

		public async Task<IEnumerable<ListingResponseDto>> ExecuteAsync(IEnumerable<ListingWithListingItemsRequest> requests)
		{
			var tasks = requests.Select(BuildListingWithItems);
			return await Task.WhenAll(tasks);
		}

	}
}
