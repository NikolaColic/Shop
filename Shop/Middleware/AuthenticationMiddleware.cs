using Infrastructure.Execution.Interfaces;
using Shop.Api.Constants;
using Shop.Service.Constants;
using System.Security.Claims;

namespace Shop.Api.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.User == null || !httpContext.User.Identity.IsAuthenticated)
            {
                _logger.LogInformation("User isn't auth");
                await _next(httpContext);
            }

            var userInfo = httpContext.RequestServices.GetService(typeof(IUserInfo)) as IUserInfo;

            if (userInfo == null)
            {
                _logger.LogInformation("IUserInfo is null");
                await _next(httpContext);
            }

            var claims = httpContext.User.Claims;

            if (claims == null || !claims.Any())
            {
                _logger.LogInformation("Claims don't exists");
                await _next(httpContext);
            }

            userInfo.Username = claims.FirstOrDefault(e => e.Type == ClaimConstants.Username)?.Value;
            userInfo.Name = claims.FirstOrDefault(e => e.Type == ClaimTypes.Name)?.Value;
            userInfo.Surname = claims.FirstOrDefault(e => e.Type == ClaimTypes.Surname)?.Value;
            userInfo.Role = claims.FirstOrDefault(e => e.Type == ClaimTypes.Role)?.Value;
            userInfo.IsAdmin = userInfo.Role == RoleConstants.Admin;

            var userId = claims.FirstOrDefault(e => e.Type == ClaimConstants.Id)?.Value;
            if (int.TryParse(userId, out var id))
            {
                userInfo.Id = id;
            }

            await _next(httpContext);
        }
    }
}
