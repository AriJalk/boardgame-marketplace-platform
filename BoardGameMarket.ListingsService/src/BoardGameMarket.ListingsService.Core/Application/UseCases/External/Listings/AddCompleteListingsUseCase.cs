using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Composition;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Identity;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.Listings;
using BoardGameMarket.ListingsService.Core.Application.Services.Helpers;
using BoardGameMarket.ListingsService.Core.Application.UseCases.External.ListingItems;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.External.Listings
{
	internal class AddCompleteListingsUseCase
	{
		private readonly IListingAdderService _listingAdderService;
		private readonly AddCompleteItemsUseCase _addCompleteListingItemsUseCase;
		private readonly ILogger<AddCompleteListingsUseCase> _logger;
		private readonly ICurrentUserContext _currentUserContext;
		private readonly ListingFetcher _listingFetcher;



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

		private async Task ValidateGroupConsistency(IEnumerable<ListingWithListingItemsRequest> requests)
		{
			Guid groupId = requests.First().ListingAddRequest.ListingGroupId;
			if (requests.Any(r => r.ListingAddRequest.ListingGroupId != groupId))
			{
				throw new InvalidOperationException("All listings must belong to the same group");
			}
			
			// Authorize
			Guid sellerId = await _listingFetcher.GetSellerUserIdByGroupIdAsync(groupId);
			IdentityHelper.AuthorizeCurrentUserOrAdmin(sellerId, _currentUserContext);
		}

		public async Task<IEnumerable<ListingResponseDto>> ExecuteAsync(IEnumerable<ListingWithListingItemsRequest> requests)
		{
			await ValidateGroupConsistency(requests);
			var tasks = requests.Select(BuildListingWithItems);
			return await Task.WhenAll(tasks);
		}
	}
}
