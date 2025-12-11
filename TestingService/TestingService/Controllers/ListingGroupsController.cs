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
			ListingItemDto listingItem = new ListingItemDto(new CatalogItemDto(Guid.NewGuid(), "test item", "bgg", "https://www.boardgamegeek.com/boardgame/1", null), "Item details");
			ListingDetailDto listing = new ListingDetailDto(Guid.NewGuid(),
					"Listing title", 500, "Listing details",
					new List<ListingItemDto>()
					{
						listingItem,
						listingItem,
					}
					);
			ListingGroupDetailedDto group = new ListingGroupDetailedDto(
				id,
				new UserSummaryDto(Guid.NewGuid(), "Test name"),
				DateTime.UtcNow,
				"Test pickup",
				"Test location",
				"Listing group details",
				new List<ListingDetailDto>()
				{
					listing,
					listing,
				}
				);

			return Ok(group);
		}
	}
}
