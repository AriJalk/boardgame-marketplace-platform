using Microsoft.AspNetCore.Mvc;
using TestingService.Dtos.ListingGroup;
using TestingService.Helpers;
using TestingService.Services;

namespace TestingService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingGroupsController : ControllerBase
	{
		ListingsService _listingsService;
		public ListingGroupsController(ListingsService listingsService)
		{
			_listingsService = listingsService;
		}
		[HttpGet("{id:guid}/detail")]
		public IActionResult GetListingGroupDetailed(Guid id)
		{
			if (_listingsService.ListingGroups.TryGetValue(id, out ListingGroupDetailDto? lg))
			{
				return Ok(lg);
			}

			return NotFound();
		}

		[HttpGet("{id:guid}/summary")]
		public IActionResult GetListingGroupSummary(Guid id)
		{
			if (_listingsService.ListingGroups.TryGetValue(id, out ListingGroupDetailDto? lg))
			{
				return Ok(lg.ToListingGroupSummary());
			}

			return NotFound();
		}



		[HttpGet()]
		public ActionResult<IEnumerable<ListingGroupSummaryDto>> Get()
		{
			return Ok(_listingsService.ListingGroups.Values.Select(lg => lg.ToListingGroupSummary()));
		}
	}
}
