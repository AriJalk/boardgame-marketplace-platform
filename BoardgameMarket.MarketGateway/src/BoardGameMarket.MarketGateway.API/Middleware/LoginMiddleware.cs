using BoardGameMarket.MarketGateway.API.Application.Users.ServiceClients;
using BoardGameMarket.MarketGateway.API.Application.Users.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
			string? auth = httpContext.Request.Headers.Authorization;
			if (Guid.TryParse(auth, out var userId))
			{
				UserInfo user = await usersDataService.GetUser(userId);
				loginService.LoggedInUser = user;
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
