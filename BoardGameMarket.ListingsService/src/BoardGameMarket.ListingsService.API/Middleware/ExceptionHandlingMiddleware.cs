using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoardGameMarket.ListingsService.API.Middleware
{
	// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionHandlingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				switch (ex)
				{
					case UnauthorizedAccessException unauthorizedException:
						httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
						await httpContext.Response.WriteAsJsonAsync(new
						{
							type = "https://httpstatuses.com/401",
							title = "Unauthorized access.",
							detail = unauthorizedException.Message,
							status = 401,
						});
						break;
					case KeyNotFoundException keyNotFoundException:
						httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
						await httpContext.Response.WriteAsJsonAsync(new
						{
							type = "https://httpstatuses.com/404",
							title = "Key not found.",
							detail = keyNotFoundException.Message,
							status = 404,
						});
						break;
					default:
						httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
						await httpContext.Response.WriteAsJsonAsync(new
						{
							type = "https://httpstatuses.com/500",
							title = "Internal server error.",
							detail = ex.Message,
							status = 500,
						});
						break;
				}
			}
		}
	}

	// Extension method used to add the middleware to the HTTP request pipeline.
	public static class ExceptionHandlingMiddlewareExtensions
	{
		public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ExceptionHandlingMiddleware>();
		}
	}
}
