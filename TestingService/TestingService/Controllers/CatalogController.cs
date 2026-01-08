using Microsoft.AspNetCore.Mvc;

namespace TestingService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CatalogController : ControllerBase
	{
		[HttpGet("bgg/{id:int}")]
		public async Task<IActionResult> GetBggItemId(int id)
		{
			if (id > 0)
			{
				return Ok(new { Id = id, Name = "catalog item name" });
			}
			return NotFound();
		}
	}
}
