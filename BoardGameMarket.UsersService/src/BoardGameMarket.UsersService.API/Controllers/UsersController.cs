using BoardGameMarket.UsersService.Core.Application.Contracts;
using BoardGameMarket.UsersService.Core.Application.Mappers;
using BoardGameMarket.UsersService.Core.Application.ServiceContracts.ApplicationUsers;
using BoardGameMarket.UsersService.Core.Application.UseCases;
using BoardGameMarket.UsersService.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.UsersService.API.Controllers
{
	[Route("api/users")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly ILogger<UsersController> _logger;
		private readonly IUserGetterService _userGetterService;
		private readonly CreateUserUseCase _createUserUseCase;

		public UsersController(ILogger<UsersController> logger, IUserGetterService userGetterService, CreateUserUseCase createUserUseCase
			)
		{
			_logger = logger;
			_userGetterService = userGetterService;
			_createUserUseCase = createUserUseCase;
		}

		[HttpGet("{userId:guid}")]
		public async Task<ActionResult<UserInfo>> GetUser(Guid userId)
		{
			_logger.LogInformation("{Controller}/{ControllerMethod}: Get user {userId}", nameof(UsersController), nameof(GetUser), userId);
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
            _logger.LogInformation("{Controller}/{ControllerMethod}: Create user {userId}", nameof(UsersController), nameof(CreateUser), createUserRequest.Id);
            UserInfo user = await _createUserUseCase.ExecuteAsync(createUserRequest);

			return Ok(user);
		}

		[HttpGet("{userId:guid}/contact-email")]
		public async Task<ActionResult<string>> GetEmail(Guid userId)
		{
            _logger.LogInformation("{Controller}/{ControllerMethod}: Get email for user {userId}", nameof(UsersController), nameof(GetEmail), userId);
            ApplicationUser? user = await _userGetterService.GetUserByUserIdAsync(userId);

			if (user == null)
			{
				return NotFound();
			}

			return Ok(user.Email);
		}

	}
}
