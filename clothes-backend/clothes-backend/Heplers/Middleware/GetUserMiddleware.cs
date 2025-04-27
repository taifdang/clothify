using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace clothes_backend.Heplers.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class GetUserMiddleware
    {
        private readonly RequestDelegate _next;

        public GetUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.StartsWithSegments("/api/cart") || httpContext.Request.Path.StartsWithSegments("/api/order"))
            {
                if (httpContext.User.Identity!.IsAuthenticated)
                {
                    httpContext.Items["IsUser"] = httpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                }
            }
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class GetUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseGetUserMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GetUserMiddleware>();
        }
    }
}
