using BoardGameMarket.ListingsService.API.Helpers;
using BoardGameMarket.ListingsService.Core.Application.Access.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Aggregates;
using BoardGameMarket.ListingsService.Core.Application.Pagination;
using BoardGameMarket.ListingsService.Core.Application.Contracts.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Filters;
using BoardGameMarket.ListingsService.Core.Application.Mappers;
using BoardGameMarket.ListingsService.Core.Domain.Aggragates;
using Microsoft.AspNetCore.Mvc;
using BoardGameMarket.ListingsService.Core.Application.UseCases.ListingGroups;
using BoardGameMarket.ListingsService.Core.Application.Queries;

namespace BoardGameMarket.ListingsService.API.Controllers
{
    [Route("api/listing-groups")]
    [ApiController]
    public class ListingGroupsController : ControllerBase
    {
        private readonly IListingGroupReadService _listingGroupReadService;
        private readonly ListingGroupAggregateBuilder _listingGroupAggregateBuilder;
        private readonly AddListingGroupUseCase _addListingGroupUseCase;
        private readonly ListingOwnershipQuery _listingOwnershipQuery;

        public ListingGroupsController(IListingGroupReadService listingGroupReadService, ListingGroupAggregateBuilder listingGroupAggregateBuilder, AddListingGroupUseCase addListingGroupUseCase, ListingOwnershipQuery listingOwnershipQuery)
        {
            _listingGroupReadService = listingGroupReadService;
            _listingGroupAggregateBuilder = listingGroupAggregateBuilder;
            _addListingGroupUseCase = addListingGroupUseCase;
            _listingOwnershipQuery = listingOwnershipQuery;
        }

        [HttpPost("page")]
        public async Task<ActionResult<PagedResult<ListingGroupSummary>>> GetPagedListingGroups([FromBody] PagedListingGroupQuery query)
        {
            Cursor? cursorObject = CursorParser.Parse(query.Cursor);
            PagedResult<ListingGroupAggregate> listingGroups = await _listingGroupAggregateBuilder.GetSummaryPageAsync(cursorObject, query.PageSize, query.Filter);
            IEnumerable<ListingGroupSummary> summaries = listingGroups.Items.Select(i => i.ToListingGroupSummary());

            PagedResult<ListingGroupSummary> pagedResult = new PagedResult<ListingGroupSummary>()
            {
                Items = summaries.ToArray(),
                NextCursor = listingGroups.NextCursor,
            };

            return Ok(pagedResult);
        }

        [HttpPost("filtered")]
        public async Task<ActionResult<ListingGroupSummary>> GetFilteredListingGroups([FromBody] ListingGroupFilter filter)
        {
            IReadOnlyCollection<ListingGroupAggregate> listingGroups = await _listingGroupAggregateBuilder.GetFilteredListingGroups(filter);
            IEnumerable<ListingGroupSummary> summaries = listingGroups.Select(lg => lg.ToListingGroupSummary());

            return Ok(summaries);
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
            Guid userId = await _listingOwnershipQuery.GetSellerUserIdByListingGroupId(listingGroupId);
            return Ok(new ListingGroupOwnershipInfo(listingGroupId, userId));
        }

        [HttpPost()]
        public async Task<ActionResult<ListingGroupDetail>> CreateListingGroup([FromBody] CreateListingGroupRequest request)
        {
            ListingGroupDetail listingGroup = await _addListingGroupUseCase.ExecuteAsync(request);

            return Ok(listingGroup);
        }
    }
}
