using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Composition;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.UseCases.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.ListingsGroups
{
	public class AddCompleteListingGroupUseCase
	{
		private readonly IListingGroupAdderService _listingGroupAdderService;
		private readonly AddCompleteListingsUseCase _addCompleteListingsUseCase;
		private readonly ILogger<AddCompleteListingGroupUseCase> _logger;

		public AddCompleteListingGroupUseCase(IListingGroupAdderService listingGroupAdderService, AddCompleteListingsUseCase addCompleteListingsUseCase, ILogger<AddCompleteListingGroupUseCase> logger)
		{
			_listingGroupAdderService = listingGroupAdderService;
			_addCompleteListingsUseCase = addCompleteListingsUseCase;
			_logger = logger;
		}


		public async Task<ListingGroupResponseDto?> ExecuteAsync(ListingGroupWithListingsRequest listingGroupWithListingsRequestDto)
		{
			ListingGroup? listingGroup = await _listingGroupAdderService.AddListingGroup(listingGroupWithListingsRequestDto.ListingGroupAddRequest);
			_logger.LogInformation($"{nameof(AddCompleteListingGroupUseCase)}:{nameof(ExecuteAsync)}- Creating listing group for seller {listingGroupWithListingsRequestDto.ListingGroupAddRequest.SellerUserId}");
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
