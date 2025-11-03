using BoardGameMarket.MarketGateway.Core.Application.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUsersServiceHttpClient _usersServiceHttpClient;
		private readonly ILogger<UsersController> _logger;

		public UsersController(IUsersServiceHttpClient usersServiceHttpClient, ILogger<UsersController> logger)
		{
			_usersServiceHttpClient = usersServiceHttpClient;
			_logger = logger;
		}

		[HttpGet("{userId:guid}")]
		public async Task<ActionResult<UserDto>> GetUserById(Guid userId)
		{
			UserDto? user = await _usersServiceHttpClient.GetUserByUserIdAsync(userId);

			if (user == null)
			{
				return NotFound();
			}

			return Ok(user);
		}
	}
}
