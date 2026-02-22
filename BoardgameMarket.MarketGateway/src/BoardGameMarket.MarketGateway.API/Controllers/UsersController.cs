using BoardGameMarket.MarketGateway.API.Application.Users.Dtos;
using BoardGameMarket.MarketGateway.API.Application.Users.Mappers;
using BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameMarket.MarketGateway.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersDataService _usersDataService;
        private readonly LoginService _loginService;

        public UsersController(UsersDataService usersDataService, LoginService loginService)
        {
            _usersDataService = usersDataService;
            _loginService = loginService;
        }

        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<UserSummaryDto>> GetUser(Guid userId)
        {
            UserInfo user = await _usersDataService.GetUser(userId);
            return Ok(user.ToUserSummaryDto());
        }

        [HttpGet("{userId:guid}/contact-email")]
        public async Task<ActionResult<string>> GetContactEmail(Guid userId)
        {
            if (_loginService.LoggedInUser == null)
            {
                return Unauthorized();
            }

            string email = await _usersDataService.GetEmail(userId);
            return Ok(email);
        }
    }
}
