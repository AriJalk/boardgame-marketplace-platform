using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Composition;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.UseCases.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingGroups
{
	public class AddCompleteListingGroupUseCase
	{
		private readonly ILogger<AddCompleteListingGroupUseCase> _logger;
		private readonly IListingGroupAdderService _listingGroupAdderService;
		private readonly AddCompleteListingsUseCase _addCompleteListingsUseCase;

		public AddCompleteListingGroupUseCase(ILogger<AddCompleteListingGroupUseCase> logger, IListingGroupAdderService listingGroupAdderService, AddCompleteListingsUseCase addCompleteListingsUseCase)
		{
			_logger = logger;
			_listingGroupAdderService = listingGroupAdderService;
			_addCompleteListingsUseCase = addCompleteListingsUseCase;
		}

		public async Task<ListingGroupResponseDto> ExecuteAsync(ListingGroupWithListingsRequest listingGroupWithListingsRequestDto)
		{
			Guid sellerUserId = listingGroupWithListingsRequestDto.ListingGroupAddRequest.SellerUserId;
			_logger.LogInformation($"{nameof(AddCompleteListingGroupUseCase)}:{nameof(ExecuteAsync)} - Creating listing group for seller {sellerUserId}");
			ListingGroup? listingGroup = await _listingGroupAdderService.AddListingGroup(listingGroupWithListingsRequestDto.ListingGroupAddRequest.ToListingGroup());
			if (listingGroup == null)
			{
				throw new InvalidOperationException("Can't create listing group");
			}
			foreach (ListingWithListingItemsRequest listingWithListingItemsRequest in listingGroupWithListingsRequestDto.ListingWithListingItemsRequests)
			{
				listingWithListingItemsRequest.ListingAddRequest.ListingGroupId = listingGroup.Id;
			}
			
			IEnumerable<ListingResponseDto> listingResponses = await _addCompleteListingsUseCase.ExecuteAsync(listingGroupWithListingsRequestDto.ListingWithListingItemsRequests);
			ListingGroupResponseDto listingGroupResponse = listingGroup.ToDto();
			listingGroupResponse.Listings = listingResponses;

			return listingGroupResponse;
		}
	}
}
