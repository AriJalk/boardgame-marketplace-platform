using BoardGameMarket.UsersService.Core.Application.DTOs;
using BoardGameMarket.UsersService.Core.Application.Mappers;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Application.UseCases;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using BoardGameMarket.UsersService.Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.UsersService.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly ILogger<UsersController> _logger;
		private readonly IUserGetterService _userGetterService;
		private readonly IUserAdderService _userAdderService;
		private readonly DeleteUserUseCase _deleteUserUseCase;
		private readonly UpdateUserDetailsUseCase _updateUserDetailsUseCase;
		private readonly UpdateUserRolesUseCase _updateUserRolesUseCase;

		public UsersController(ILogger<UsersController> logger, IUserGetterService userGetterService, IUserAdderService userAdderService, DeleteUserUseCase deleteUserUseCase, UpdateUserDetailsUseCase updateUserDetailsUseCase, UpdateUserRolesUseCase updateUserRolesUseCase)
		{
			_logger = logger;
			_userGetterService = userGetterService;
			_userAdderService = userAdderService;
			_deleteUserUseCase = deleteUserUseCase;
			_updateUserDetailsUseCase = updateUserDetailsUseCase;
			_updateUserRolesUseCase = updateUserRolesUseCase;
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
			if (await _userGetterService.GetUserByUserIdAsync(userDto.UserId) != null)
			{
				return Conflict("User exists");
			}
			ApplicationUser userToAdd = userDto.ToApplicationUser();
			userToAdd.Role = RoleType.User;
			ApplicationUser? userFromService = await _userAdderService.AddUserAsync(userToAdd);
			if (userFromService == null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
			return CreatedAtAction(nameof(GetUser), new { userId = userFromService.UserId }, userFromService.ToDto());
		}

		[HttpDelete("{targetId:guid}")]
		public async Task<IActionResult> DeleteUser(Guid targetId, [FromQuery] Guid requesterId)
		{
			if (await _deleteUserUseCase.ExecuteAsync(requesterId, targetId))
			{
				return NoContent();
			}
			return NotFound();
		}

		[HttpPut("{targetId:guid}")]
		public async Task<ActionResult<UserDto>> UpdateUserDetails([FromRoute] Guid targetId, [FromQuery] Guid requesterId, [FromBody] UserUpdateRequest userUpdateRequest)
		{
			UserDto? user = await _updateUserDetailsUseCase.ExecuteAsync(requesterId, targetId, userUpdateRequest);
			if (user == null)
			{
				return BadRequest();
			}
			return Ok(user);
		}

		[HttpPut("{targetId:guid}/role")]
		public async Task<ActionResult<UserDto>> UpdateUserRole([FromRoute] Guid targetId, [FromQuery] Guid requesterId, [FromQuery] RoleType roleTypes)
		{
			UserDto? updatedUser = await _updateUserRolesUseCase.ExecuteAsync(requesterId, targetId, roleTypes);

			return Ok(updatedUser);
		}
	}
}
