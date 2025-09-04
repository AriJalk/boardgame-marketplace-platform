using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.Services.UseCases
{
	public class GetListingGroupUseCase
	{
		private readonly GetListingsUseCase _getListingUseCase;
		private readonly IListingGroupGetterService _listingGroupGetterService;

		private async Task<ListingGroupResponseDto> BuildListingGroupDtoAsync(Guid listingGroupId, IEnumerable<ListingResponseDto> listingDtos)
		{
			ListingGroup? listingGroup = await _listingGroupGetterService.GetListingGroupByIdAsync(listingGroupId);
			if (listingGroup == null)
			{
				throw new KeyNotFoundException($"ListingGroup {listingGroupId} not found");
			}

			ListingGroupResponseDto listingGroupDto = listingGroup.ToDto();
			listingGroupDto.Listings = listingDtos;

			return listingGroupDto;
		}

		public async Task<ListingGroupResponseDto> ExecuteByListingGroupIdAsync(Guid listingGroupId)
		{
			IEnumerable<ListingResponseDto> listingDtos = await _getListingUseCase.ExecuteByListingGroupIdAsync(listingGroupId);

			return await BuildListingGroupDtoAsync(listingGroupId, listingDtos);
		}

		public async Task<ListingGroupResponseDto> ExecuteByListingIdAsync(Guid listingId)
		{
			ListingResponseDto listingDto = await _getListingUseCase.ExecuteByListingIdAsync(listingId);

			Guid? listingGroupId = listingDto.ListingGroupId;
			ArgumentNullException.ThrowIfNull(listingGroupId);

			return await BuildListingGroupDtoAsync(listingGroupId.Value, new List<ListingResponseDto> { listingDto });
		}
	}
}
