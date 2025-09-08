using BoardGameMarket.UsersService.Core.Application.DTOs;
using BoardGameMarket.UsersService.Core.Application.Mappers;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.UsersService.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserGetterService _userGetterService;

		public UsersController(IUserGetterService userGetterService)
		{
			_userGetterService = userGetterService;
		}

		[HttpGet("[action]")]
		public async Task<ActionResult<UserDTO>> GetUser(Guid userId)
		{
			ApplicationUser? user = await _userGetterService.GetUserByUserIdAsync(userId);
			if (user == null)
			{
				return NotFound();
			}

			return Ok(user.ToDTO());
		} 
	}
}
