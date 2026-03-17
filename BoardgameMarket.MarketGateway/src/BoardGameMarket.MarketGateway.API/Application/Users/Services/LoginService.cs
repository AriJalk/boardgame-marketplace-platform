using BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients;
using BoardGameMarket.MarketGateway.API.Helpers;
using System.Security.Claims;

namespace BoardGameMarket.MarketGateway.API.Application.Users.Services
{
    public class LoginService
    {
        private UserInfo? _loggedInUser;
        private IHttpContextAccessor _httpContextAccessor;
        private UsersDataService _usersDataService;


        public LoginService(IHttpContextAccessor httpContextAccessor, UsersDataService usersDataService)
        {
            _httpContextAccessor = httpContextAccessor;
            _usersDataService = usersDataService;
        }

        public async Task<Guid> GetUserIdOrThrow()
        {
            if (_loggedInUser == null)
            {
                UserInfo? userInfo = await GetCurrentUser();
                if (userInfo == null)
                {
                    throw new InvalidOperationException("User not logged in");
                }
                _loggedInUser = userInfo;
            }

            return _loggedInUser.Id;
        }

        public async Task<UserInfo?> GetCurrentUser()
        {
            ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;

            if (user == null)
            {
                return null;
            }

            UserInfo? userInfo = null;
            string? oid = user.FindFirst(EntraClaims.ObjectId)?.Value;
            if (oid != null && Guid.TryParse(oid, out Guid userId))
            {
                userInfo = await _usersDataService.GetUser(userId);

                if (userInfo != null)
                {
                    return userInfo;
                }
                string? displayName = user.FindFirst("name")?.Value;
                string? email = user.FindFirst("preferred_username")?.Value;
                string? firstName = user.FindFirst(ClaimTypes.GivenName)?.Value;
                string? lastName = user.FindFirst(ClaimTypes.Surname)?.Value;

                if (displayName == null || email == null || firstName == null || lastName == null)
                {
                    throw new InvalidOperationException("Missing user info");
                }
                CreateUserRequest request = new CreateUserRequest(userId, firstName, lastName, displayName, email);
                try
                {
                    userInfo = await _usersDataService.CreateUser(request);
                }
                catch
                {
                    userInfo = await _usersDataService.GetUser(userId);
                    if (userInfo == null)
                    {
                        throw new InvalidOperationException("Can't lazy load user");
                    }
                }
            }
            return userInfo;
        }
    }
}
