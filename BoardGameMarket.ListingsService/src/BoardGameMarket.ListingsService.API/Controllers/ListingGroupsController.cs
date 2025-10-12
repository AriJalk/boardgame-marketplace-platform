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
		private readonly AddCompleteListingGroupUseCase _addCompleteListingGroupUseCase;
		private readonly DeleteListingGroupUseCase _deleteListingGroupUseCase;

		public ListingGroupsController(GetListingGroupsPageUseCase getListingGroupsPageUseCase, AddCompleteListingGroupUseCase addCompleteListingGroupUseCase, DeleteListingGroupUseCase deleteListingGroupUseCase)
		{
			_getListingGroupsPageUseCase = getListingGroupsPageUseCase;
			_addCompleteListingGroupUseCase = addCompleteListingGroupUseCase;
			_deleteListingGroupUseCase = deleteListingGroupUseCase;
		}

		[HttpGet]
		public async Task<ActionResult<PagedResult<ListingGroupResponseDto>>> GetListingGroups([FromQuery] ListingGroupFilter? filter, [FromHeader] string? cursor, [FromHeader] int pageSize = 10)
		{
			Cursor? cursorObject = CursorParser.Parse(cursor);
			return Ok(await _getListingGroupsPageUseCase.ExecuteAsync(cursorObject, pageSize, filter));
		}

		[HttpPost]
		public async Task<ActionResult<ListingGroupResponseDto>> AddListingGroupWithListings(ListingGroupWithListingsRequest listingGroupWithListingsRequest)
		{
			ListingGroupResponseDto listingGroupResponseDto = await _addCompleteListingGroupUseCase.ExecuteAsync(listingGroupWithListingsRequest);
			return Ok(listingGroupResponseDto);
		}

		[HttpDelete("{listingGroupId:guid}")]
		public async Task<IActionResult> DeleteListingGroup(Guid listingGroupId)
		{
			bool isDeleted = await _deleteListingGroupUseCase.ExecuteAsync(listingGroupId);
			if (isDeleted)
			{
				return NoContent();
			}
			return NotFound();
		}
	}
}
