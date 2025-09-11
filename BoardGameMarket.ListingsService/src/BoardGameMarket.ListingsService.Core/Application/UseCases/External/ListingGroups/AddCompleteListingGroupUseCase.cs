using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Composition;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Identity;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.UseCases.External.Listings;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BoardGameMarket.ListingsService.Core.Application.UseCases.External.ListingGroups
{
	internal class AddCompleteListingGroupUseCase
	{
		private readonly ILogger<AddCompleteListingGroupUseCase> _logger;
		private readonly IListingGroupAdderService _listingGroupAdderService;
		private readonly AddCompleteListingsUseCase _addCompleteListingsUseCase;
		private readonly ICurrentUserContext _currentUserContext;

		public AddCompleteListingGroupUseCase(ILogger<AddCompleteListingGroupUseCase> logger, IListingGroupAdderService listingGroupAdderService, AddCompleteListingsUseCase addCompleteListingsUseCase, ICurrentUserContext currentUserContext)
		{
			_logger = logger;
			_listingGroupAdderService = listingGroupAdderService;
			_addCompleteListingsUseCase = addCompleteListingsUseCase;
			_currentUserContext = currentUserContext;
		}

		public async Task<ListingGroupResponseDto?> ExecuteAsync(ListingGroupWithListingsRequest listingGroupWithListingsRequestDto)
		{
			Guid sellerUserId = listingGroupWithListingsRequestDto.ListingGroupAddRequest.SellerUserId;
			IdentityHelper.AuthorizeCurrentUserOrAdmin(sellerUserId, _currentUserContext);
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
