using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Composition;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingItems;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.Listings
{
	public class AddCompleteListingsUseCase
	{
		private readonly IListingAdderService _listingAdderService;
		private readonly AddCompleteListingItemsUseCase _addCompleteListingItemsUseCase;
		private readonly ILogger<AddCompleteListingsUseCase> _logger;
		private readonly ListingFetcher _listingFetcher;

		public AddCompleteListingsUseCase(IListingAdderService listingAdderService, AddCompleteListingItemsUseCase addCompleteListingItemsUseCase, ILogger<AddCompleteListingsUseCase> logger, ListingFetcher listingFetcher)
		{
			_listingAdderService = listingAdderService;
			_addCompleteListingItemsUseCase = addCompleteListingItemsUseCase;
			_logger = logger;
			_listingFetcher = listingFetcher;
		}

		private async Task ValidateGroupConsistency(IEnumerable<ListingWithListingItemsRequest> requests)
		{
			Guid groupId = requests.First().ListingAddRequest.ListingGroupId;


			if (requests.Any(r => r.ListingAddRequest.ListingGroupId != groupId || r.ListingAddRequest.ListingGroupId == Guid.Empty))
			{
				throw new InvalidOperationException("All listings must belong to the same existing group");
			}

			// Validate group existance
			await _listingFetcher.GetListingGroupByIdAsync(groupId);
		}


		private async Task<ListingResponseDto> BuildListingWithItems(ListingWithListingItemsRequest request)
		{
			var listing = await _listingAdderService.AddListingAsync(request.ListingAddRequest.ToListing());
			if (listing == null)
			{
				_logger.LogError($"{nameof(AddCompleteListingsUseCase)}:{nameof(BuildListingWithItems)} - can't add listing");
				throw new InvalidOperationException("Failed to create listing");
			}

			foreach (var itemRequest in request.ListingItemAddRequests)
			{
				itemRequest.ListingId = listing.Id;
			}

			var itemResponses = await _addCompleteListingItemsUseCase.ExecuteAsync(request.ListingItemAddRequests);

			var response = listing.ToDto();
			response.ListingItems = itemResponses;
			return response;
		}

		public async Task<IEnumerable<ListingResponseDto>> ExecuteAsync(IEnumerable<ListingWithListingItemsRequest> requests)
		{
			ValidateGroupConsistency(requests);
			var tasks = requests.Select(BuildListingWithItems);
			return await Task.WhenAll(tasks);
		}
	}
}
