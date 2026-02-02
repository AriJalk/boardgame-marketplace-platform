using BoardGameMarket.ListingsService.API.Helpers;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Aggregates;
using BoardGameMarket.ListingsService.Core.Application.Common;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Domain.Aggragates;
using BoardGameMarket.ListingsService.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.ListingsService.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingGroupsController : ControllerBase
	{
		private IListingGroupReadService _listingGroupReadService;
		private ListingGroupAggregateBuilder _listingGroupAggregateBuilder;

		public ListingGroupsController(IListingGroupReadService listingGroupReadService, ListingGroupAggregateBuilder listingGroupAggregateBuilder)
		{
			_listingGroupReadService = listingGroupReadService;
			_listingGroupAggregateBuilder = listingGroupAggregateBuilder;
		}

		[HttpGet()]
		public async Task<ActionResult<PagedResult<ListingGroupSummary>>> Get([FromQuery] string? cursor, [FromQuery] int pageSize = 10, [FromQuery] ListingGroupFilter? filter = null)
		{
			Cursor? cursorObject = CursorParser.Parse(cursor);
			PagedResult<ListingGroupAggregate> listingGroups = await _listingGroupAggregateBuilder.GetSummaryPageAsync(cursorObject, pageSize, filter);
			IEnumerable<ListingGroupSummary> summaries = listingGroups.Items.Select(i => i.ToListingGroupSummary());

			PagedResult<ListingGroupSummary> pagedResult = new PagedResult<ListingGroupSummary>()
			{
				Items = summaries.ToArray(),
				NextCursor = listingGroups.NextCursor,
			};

			return Ok(pagedResult);
		}


		[HttpGet("{listingGroupId:guid}")]
		public async Task<ActionResult<ListingGroupDetail>> GetListingGroup(Guid listingGroupId)
		{
			ListingGroupAggregate? listingGroupAggregate;
			try
			{
				listingGroupAggregate = await _listingGroupAggregateBuilder.GetListingGroupAggregateAsync(listingGroupId, loadItems: true, loadQueues: false);


				return Ok(listingGroupAggregate.ToListingGroupDetail());
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpGet("{listingGroupId:guid}/ownership")]
		public async Task<ActionResult<ListingGroupOwnershipInfo>> GetListingGroupOwnership(Guid listingGroupId)
		{
			ListingGroup? listingGroup = await _listingGroupReadService.GetListingGroupByIdAsync(listingGroupId);
			if (listingGroup == null)
			{
				return NotFound();
			}
			return Ok(new ListingGroupOwnershipInfo(listingGroupId, listingGroup.SellerUserId));
		}
	}
}
