using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.Services.ListingGroups;
using BoardGameMarket.ListingsService.Core.Domain.Entities;

namespace BoardGameMarket.ListingsService.Core.Application.Services.Assemblers.DTOs
{
	internal class ListingGroupDtoComposer
	{
		private readonly ListingGroupGetterService _listingGroupGetterService;
		private readonly ListingDtoComposer _listingDtoComposer;

		public async Task<ListingGroupResponseDto> ComposeListingGroupAsync(Guid listingGroupId)
		{
			ListingGroup? listingGroup = await _listingGroupGetterService.GetListingGroupByIdAsync(listingGroupId);
			if (listingGroup == null)
			{
				throw new KeyNotFoundException($"ListingGroup {listingGroupId} not found");
			}

			ListingGroupResponseDto listingGroupDto = listingGroup.ToDto();
			listingGroupDto.Listings = await _listingDtoComposer.ComposeByGroupIdAsync(listingGroupId);

			return listingGroupDto;
		}
	}
}
