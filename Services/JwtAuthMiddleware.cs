 namespace DiplomApi.Services
{
    public class JwtAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtAuthMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Invoke(HttpContext context)
        {

            string token = context.Request.Cookies["token"];

            if (!string.IsNullOrEmpty(token))
            {
                //context.Request.Headers.Add("Authorization",$"Bearer {token}");
            }

            await _next(context);
        }
    }
}
