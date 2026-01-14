using BoardGameMarket.ListingsService.API.Helpers;
using BoardGameMarket.ListingsService.Core.Application.Common;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Application.Services.AggregateServices;
using BoardGameMarket.ListingsService.Core.Domain.Aggragates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.ListingsService.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingGroupsController : ControllerBase
	{
		private ListingGroupAggregateService _aggregateService;

		public ListingGroupsController(ListingGroupAggregateService aggregateService)
		{
			_aggregateService = aggregateService;
		}

		[HttpGet()]
		public async Task<ActionResult<PagedResult<ListingGroupSummary>>> Get([FromQuery] string? cursor, [FromQuery] int pageSize = 10, [FromQuery] ListingGroupFilter? filter = null)
		{
			Cursor? cursorObject = CursorParser.Parse(cursor);
			PagedResult<ListingGroupAggregate> listingGroups = await _aggregateService.GetSummaryPageAsync(cursorObject, pageSize, filter);
			IEnumerable<ListingGroupSummary> summaries = listingGroups.Items.Select(i => i.ToListingGroupSummary());

			PagedResult<ListingGroupSummary> pagedResult = new PagedResult<ListingGroupSummary>()
			{
				Items = summaries.ToArray(),
				NextCursor = listingGroups.NextCursor,
			};

			return Ok(pagedResult);
		}


		[HttpGet("/{listingGroupId:guid}")]
		public async Task<ActionResult<ListingGroupDetail>> GetListingGroup(Guid listingGroupId)
		{
			ListingGroupAggregate? listingGroupAggregate;
			try
			{
				listingGroupAggregate = await _aggregateService.GetListingGroupAggregateAsync(listingGroupId, loadItems: true, loadQueues: false);


				return Ok(listingGroupAggregate.ToListingGroupDetail());
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}
	}
}
