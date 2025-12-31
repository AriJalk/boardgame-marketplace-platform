using Microsoft.AspNetCore.Mvc;
using TestingService.Dtos;
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
		[HttpGet("{id:guid}")]
		public IActionResult Get(Guid id)
		{
			if (_listingsService.ListingGroups.TryGetValue(id, out ListingGroupDetailedDto? lg))
			{
				return Ok(lg);
			}

			return NotFound();
		}

		[HttpGet()]
		public IActionResult Get()
		{
			return Ok(_listingsService.ListingGroups.Values);
		}
	}
}
