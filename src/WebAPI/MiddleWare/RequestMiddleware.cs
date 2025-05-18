namespace WebAPI.MiddleWare
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies["X-Access-Token"];

            if (!string.IsNullOrEmpty(token) &&
                !context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Request.Headers.Append("Authorization", $"Bearer {token}");
            }

            await _next(context);
        }
    }
}
