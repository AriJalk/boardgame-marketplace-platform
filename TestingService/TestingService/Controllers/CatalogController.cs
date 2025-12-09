using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestingService.Dtos;

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
				return Ok(Guid.NewGuid());
			}
			return NotFound();
		}
	}
}
