using BoardGameMarket.ListingsService.Core.Application.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.API.Middleware
{
	// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
	public class CurrentUserMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ICurrentUserContext _currentUserContext;

		public CurrentUserMiddleware(RequestDelegate next, ICurrentUserContext currentUserContext)
		{
			_next = next;
			_currentUserContext = currentUserContext;
		}

		public Task Invoke(HttpContext httpContext)
		{
			string? userIdHeader = httpContext.Request.Headers["User-Id"];
			string? rolesHeader = httpContext.Response.Headers["Roles"];

			if (Guid.TryParse(userIdHeader, out Guid userId))
			{
				var roles = rolesHeader?.Split(',') ?? Array.Empty<string>();
				_currentUserContext.Set(userId, roles);
			}


			return _next(httpContext);
		}
	}

	// Extension method used to add the middleware to the HTTP request pipeline.
	public static class CurrentUserMiddlewareExtensions
	{
		public static IApplicationBuilder UseCurrentUserMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<CurrentUserMiddleware>();
		}
	}
}
