using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestingService
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestingController : ControllerBase
	{
		public class CatalogItemDto
		{
			public Guid Id { get; set; }
			public string Name { get; set; } = null!;
			public string? Type { get; set; }
			public string? BggLink { get; set; }
			public string? ImageUrl { get; set; }
		}

		[HttpGet("[action]")]
		public async Task<IActionResult> GetListing()
		{
			List<CatalogItemDto> catalogItems = new List<CatalogItemDto>
			{
				new() {Id = Guid.NewGuid(), Name= "item1", Type="bgg", BggLink="http://www.boardgamegeek.com"},
				new() {Id = Guid.NewGuid(), Name= "item2", Type="custom"},
			};
			var listingItems = new[]
			{
				new {CatalogItem = catalogItems[0], Details = "Item1 details"},
				new {CatalogItem = catalogItems[1], Details = "Item2 details"},
			};
			var listing = new
			{
				Id = Guid.NewGuid(),
				Title = "Listing title",
				Price = 50,
				Details = "My listing",
				Items = listingItems,
			};

			return Ok(listing);
		}

		[HttpGet("items/bgg/{id:int}")]
		public async Task<IActionResult> GetBggItemId(int id)
		{
			if (id > 100)
			{
				return Ok(Guid.NewGuid());
			}
			return NotFound();
		}
	}
}
