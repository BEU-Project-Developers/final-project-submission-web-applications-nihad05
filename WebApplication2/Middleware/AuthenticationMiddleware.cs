public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower() ?? "";

        // Only allow these specific paths without authentication
        var allowedPaths = new[]
        {
            "/login",
            "/css/",
            "/js/",
            "/images/",
            "/lib/",
            "/favicon.ico"
        };

        // Check if the current path is allowed
        bool isAllowedPath = allowedPaths.Any(allowedPath => path.StartsWith(allowedPath));

        if (isAllowedPath)
        {
            await _next(context);
            return;
        }

        // Check if user is authenticated
        if (!context.User.Identity.IsAuthenticated)
        {
            context.Response.Redirect("/Login");
            return;
        }

        await _next(context);
    }
}