using Azure.Core;
using clothes_backend.Data;
using clothes_backend.DTO.PRODUCT_DTO;
using clothes_backend.Utils;
using Microsoft.Extensions.Caching.Distributed;
using System.IdentityModel.Tokens.Jwt;
namespace clothes_backend.Heplers.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CheckTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public  async Task Invoke(HttpContext httpContext,DatabaseContext db,IDistributedCache distributedCache)
        {
            var access_token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ","");      
            if (!string.IsNullOrEmpty(access_token))
            {
                var check_token = new JwtSecurityTokenHandler().ReadToken(access_token) as JwtSecurityToken;
                if (check_token?.ValidTo <= DateTime.UtcNow)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await httpContext.Response.WriteAsync($"Token is expired {check_token.ValidTo}");
                    return;
                }
                //var isBlockToken = db.blacklist_token.Any(x => x.token == access_token);
                if(distributedCache.TryGetValue($"bl_{access_token}", out string? token))
                {
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await httpContext.Response.WriteAsync("Token is revoked");
                    return;
                }           
            }
            await _next(httpContext);
        }
    }
    // Extension method used to AddBase the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckTokenMiddleware>();
        }
    }
}
