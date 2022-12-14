using Data.Entities;
using Infrastructure.Service.Interfaces;
using Infrastructure.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Service.Interfaces.Proxy;

namespace Shop.Api.Controllers
{
    /// <summary>
    /// User controller for autentication
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly IProxyService<User> _userService;

        public UserController(IAuthService service, IProxyService<User> userService)
        {
            _service = service;
            _userService = userService;
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var article = await _userService.GetById(id);
            return Ok(article);
        }

        /// <summary>
        /// Get access and refresh token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("authorize")]
        public async Task<ActionResult<IEnumerable<Article>>> Authorize([FromBody] LoginDto login)
        {
            var token = await _service.Authorize(login.Username, login.Password);

            if (token == null)
            {
                return NotFound();
            }

            return Ok(token);
        }

        /// <summary>
        /// Generate new access and refresh token
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<Article>> GenerateToken(TokenModelDto tokenModel)
        {
            var token = await _service.GenerateRefreshToken(tokenModel);
            return Ok(token);
        }

        /// <summary>
        /// Revoke refresh tokens for specific user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Policy = "Admin")]
        [HttpPost("{id}/revoke")]
        public async Task<ActionResult<Article>> RevokeById(int id)
        {
            var status = await _service.Revoke(id);
            return Ok(status);
        }

        /// <summary>
        /// Revoke refresh tokens for all users
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "Admin")]
        [HttpPost("revoke")]
        public async Task<ActionResult<Article>> RevokeAll()
        {
            var status = await _service.RevokeAll();
            return Ok(status);
        }
    }
}
