using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CatalogController : ControllerBase
	{
		[HttpGet("bgg/{id:int}")]
		public async Task<IActionResult> GetBggItemId(int id)
		{
			await Task.Delay(1000);
			if (id > 0)
			{
				return Ok(new { Id = Guid.NewGuid(), Name = "catalog item name" });
			}
			return NotFound();
		}
	}
}
