using BoardGameMarket.ListingsService.API.Helpers;
using BoardGameMarket.ListingsService.Core.Application.DTOs.Responses;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.ServiceContracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.UseCases.External.ListingGroups;
using BoardGameMarket.ListingsService.Core.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.ListingsService.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingGroupsController : ControllerBase
	{
		private readonly GetListingGroupsPageUseCase _getListingGroupsPageUseCase;

		public ListingGroupsController(GetListingGroupsPageUseCase getListingGroupsPageUseCase)
		{
			_getListingGroupsPageUseCase = getListingGroupsPageUseCase;
		}

		[HttpGet]
		public async Task<ActionResult<PagedResult<ListingGroupResponseDto>>> GetListingGroups([FromHeader] string? cursor, [FromHeader] int pageSize, [FromQuery] ListingGroupFilter? filter)
		{
			Cursor? cursorObject = CursorParser.Parse(cursor);
			return Ok(await _getListingGroupsPageUseCase.ExecuteAsync(cursorObject, pageSize, filter));
		}
	}
}
