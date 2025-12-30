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
						listingItem with {CatalogItem = listingItem.CatalogItem with {Id = Guid.NewGuid()}},
					}
					);
			ListingGroupDetailedDto group = new ListingGroupDetailedDto(
				id,
				new UserSummaryDto(Guid.Parse("4552ea90-84c8-4d7c-8409-d80b39cad771"), "Test name"),
				DateTime.UtcNow,
				"Test pickup",
				"Test location",
				"Listing group details",
				new List<ListingDetailDto>()
				{
					listing,
					listing with {Id = Guid.NewGuid()},
				}
				);

			return Ok(group);
		}
	}
}
