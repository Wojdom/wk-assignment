using System;
using System.Threading.Tasks;
using Common.Domain.ActionContext;
using Common.Domain.User.ValueObjects;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace WebApi.Middlewares
{
	public class UserIdentificatorMiddleware
    {
        private readonly RequestDelegate _next;

        public UserIdentificatorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IActionContextProvider actionContextProvider)
        {
            if (actionContextProvider == null)
            {
                throw new ArgumentException(nameof(actionContextProvider));
            }

            bool tryGetResult = context.Request.Headers.TryGetValue("userId", out StringValues userId);
            if (tryGetResult)
            {
                actionContextProvider.RegisterContext(
                    new ActionContext(new UserId(Guid.Parse(userId))));
            }

            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }

    public static class UserIdentificatorMiddlewareMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserIdentificator(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserIdentificatorMiddleware>();
        }
    }
}

