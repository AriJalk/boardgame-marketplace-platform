using BoardGameMarket.UsersService.Core.Application.Contracts;
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
		private readonly CreateUserUseCase _createUserUseCase;

		public UsersController(ILogger<UsersController> logger, IUserGetterService userGetterService, IUserAdderService userAdderService, CreateUserUseCase createUserUseCase
			)
		{
			_logger = logger;
			_userGetterService = userGetterService;
			_userAdderService = userAdderService;
			_createUserUseCase = createUserUseCase;
		}

		[HttpGet("{userId:guid}")]
		public async Task<ActionResult<UserInfo>> GetUser(Guid userId)
		{
			ApplicationUser? user = await _userGetterService.GetUserByUserIdAsync(userId);
			if (user == null)
			{
				return NotFound();
			}
			return Ok(user.ToUserInfo());
		}

		[HttpPost()]
		public async Task<ActionResult<UserInfo>> CreateUser(CreateUserRequest createUserRequest)
		{
			UserInfo user = await _createUserUseCase.ExecuteAsync(createUserRequest);

			return Ok(user);
		}



	}
}
