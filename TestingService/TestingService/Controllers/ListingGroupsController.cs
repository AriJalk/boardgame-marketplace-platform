using Microsoft.AspNetCore.Mvc;
using TestingService.Dtos;

namespace TestingService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingGroupsController : ControllerBase
	{
		[HttpGet("{id:guid}")]
		public IActionResult Get(Guid id)
		{
			ListingGroupDetailedDto group = new ListingGroupDetailedDto(
				id,
				new UserSummaryDto(Guid.NewGuid(), "Test name"),
				DateTime.UtcNow,
				"Test pickup",
				"Test location",
				"Listing group details",
				new List<ListingDetailDto>()
				{
					new ListingDetailDto(Guid.NewGuid(),
					"Listing title", 500, "Listing details",
					new List<ListingItemDto>()
					{
						new ListingItemDto(new CatalogItemDto(Guid.NewGuid(), "test item", "bgg", "https://www.boardgamegeek.com", null), "Item details")
					}
					)
				}
				);

			return Ok(group);
		}
	}
}
