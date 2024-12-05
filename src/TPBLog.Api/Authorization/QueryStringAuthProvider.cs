namespace TPBlog.Api.Authorization
{
    public class QueryStringAuthProvider
    {
        private readonly RequestDelegate _next;

        public QueryStringAuthProvider(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Kiểm tra token trong query string
            var token = context.Request.Query["access_token"].FirstOrDefault();
            if (!string.IsNullOrEmpty(token))
            {
                // Đặt token vào header Authorization nếu có
                context.Request.Headers["Authorization"] = "Bearer " + token;
            }

            await _next(context);
        }
    }
}
