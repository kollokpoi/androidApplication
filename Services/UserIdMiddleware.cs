using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DiplomApi.Services
{
    public class UserIdMiddleware 
    {
        private readonly RequestDelegate _next;

        public UserIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                context.Items["UserId"] = userId;
            }

            await _next(context);
        }
    }
}
