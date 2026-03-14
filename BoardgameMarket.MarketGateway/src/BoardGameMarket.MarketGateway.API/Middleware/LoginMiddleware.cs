using BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;
using BoardGameMarket.MarketGateway.API.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BoardGameMarket.MarketGateway.API.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoginMiddleware
    {
        private readonly RequestDelegate _next;
        public LoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext httpContext, UsersDataService usersDataService, LoginService loginService)
        {
            ClaimsPrincipal user = httpContext.User;
            if (!user.Identity?.IsAuthenticated ?? true)
            {
                await _next(httpContext);
                return;
            }
            string? oid = user.FindFirst(EntraClaims.ObjectId)?.Value;
            if (oid != null && Guid.TryParse(oid, out Guid userId))
            {
                UserInfo? userInfo = await usersDataService.GetUser(userId);

                if (userInfo == null)
                {

                    string? displayName = user.FindFirst("name")?.Value;
                    string? email = user.FindFirst("preferred_username")?.Value;
                    string? firstName = user.FindFirst(ClaimTypes.GivenName)?.Value;
                    string? lastName = user.FindFirst(ClaimTypes.Surname)?.Value;

                    if (displayName != null && email != null && firstName != null && lastName != null)
                    {
                        CreateUserRequest request = new CreateUserRequest(userId, firstName, lastName, displayName, email);
                        userInfo = await usersDataService.CreateUser(request);
                    }
                }

                loginService.LoggedInUser = userInfo;
            }

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoginMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoginMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoginMiddleware>();
        }
    }
}
