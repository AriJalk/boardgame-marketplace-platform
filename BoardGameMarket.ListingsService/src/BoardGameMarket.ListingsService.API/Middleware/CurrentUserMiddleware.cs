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
			string? roleHeader = httpContext.Request.Headers["Role"];

			if (Guid.TryParse(userIdHeader, out Guid userId) && int.TryParse(roleHeader, out int roleInt) && Enum.IsDefined(typeof(RoleType), roleInt))
			{
				_currentUserContext.Set(userId, (RoleType)roleInt);
			}
			else
			{
				_currentUserContext.Set(null, null);
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
