using BoardGameMarket.ListingsService.API.Helpers;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Requests.Composition;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingGroups;
using BoardGameMarket.ListingsService.Core.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.ListingsService.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingGroupsController : ControllerBase
	{
		private readonly GetListingGroupsPageUseCase _getListingGroupsPageUseCase;

		public ListingGroupsController(GetListingGroupsPageUseCase getListingGroupsPageUseCase, AddCompleteListingGroupUseCase addCompleteListingGroupUseCase)
		{
			_getListingGroupsPageUseCase = getListingGroupsPageUseCase;
		}

		[HttpGet]
		public async Task<ActionResult<PagedResult<ListingGroupResponseDto>>> GetListingGroups([FromHeader] string? cursor, [FromHeader] int pageSize, [FromQuery] ListingGroupFilter? filter)
		{
			Cursor? cursorObject = CursorParser.Parse(cursor);
			return Ok(await _getListingGroupsPageUseCase.ExecuteAsync(cursorObject, pageSize, filter));
		}

		[HttpPost]
		public async Task<ActionResult<ListingGroupResponseDto>> AddListingGroupWithListings(AddCompleteListingGroupUseCase addCompleteListingGroupUseCase, ListingGroupWithListingsRequest listingGroupWithListingsRequest)
		{
			ListingGroupResponseDto listingGroupResponseDto = await addCompleteListingGroupUseCase.ExecuteAsync(listingGroupWithListingsRequest);
			return Ok(listingGroupResponseDto);
		}
	}
}
