using BoardGameMarket.UsersService.Core.Application.DTOs;
using BoardGameMarket.UsersService.Core.Application.Mappers;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Application.UseCases;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.UsersService.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserGetterService _userGetterService;
		private readonly IUserAdderService _userAdderService;
		private readonly DeleteUserUseCase _deleteUserUseCase;

		public UsersController(IUserGetterService userGetterService, IUserAdderService userAdderService, DeleteUserUseCase deleteUserUseCase)
		{
			_userGetterService = userGetterService;
			_userAdderService = userAdderService;
			_deleteUserUseCase = deleteUserUseCase;
		}

		[HttpGet("{userId:guid}")]
		public async Task<ActionResult<UserDto>> GetUser(Guid userId)
		{
			ApplicationUser? user = await _userGetterService.GetUserByUserIdAsync(userId);
			if (user == null)
			{
				return NotFound();
			}

			return Ok(user.ToDto());
		}

		[HttpPost]
		public async Task<ActionResult<UserDto>> RegisterUser(UserDto userDto)
		{
			ApplicationUser? user = await _userAdderService.AddUserAsync(userDto.ToApplicationUser());
			if (user == null)
			{
				return BadRequest();
			}

			return Ok(user.ToDto());
		}

		[HttpDelete("{targetId:guid}")]
		public async Task<IActionResult> DeleteUser(Guid targetId, [FromQuery] Guid requesterId)
		{
			ApplicationUser? requesterUser = await _userGetterService.GetUserByUserIdAsync(targetId);
			if (requesterUser == null)
			{
				return BadRequest();
			}

			if (await _deleteUserUseCase.ExecuteAsync(requesterUser, targetId))
			{
				return NoContent();
			}

			return NotFound();
		}
	}
}
