namespace Shop.Api.Middleware
{
    public static class AuthenticationMiddlewareExtension
    {
        public static void ConfigureAuthenticationMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
