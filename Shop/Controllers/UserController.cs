using Data.Entities;
using Infrastructure.Service.Interfaces;
using Infrastructure.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Api.Controllers
{
    /// <summary>
    /// User controller for autentication
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _service;

        public UserController(IAuthService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get access and refresh token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("authorize")]
        public async Task<ActionResult<IEnumerable<Article>>> Authorize([FromBody] string username, [FromBody] string password)
        {
            var token = await _service.Authorize(username, password);

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

            if (token == null)
            {
                return NotFound();
            }

            return Ok(token);
        }

        /// <summary>
        /// Revoke refresh tokens for specific user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/revoke")]
        public async Task<ActionResult<Article>> RevokeById(int id)
        {
            var status = await _service.Revoke(id);

            if (status)
            {
                return NotFound();
            }

            return Ok(status);
        }

        /// <summary>
        /// Revoke refresh tokens for all users
        /// </summary>
        /// <returns></returns>
        [HttpPost("revoke")]
        public async Task<ActionResult<Article>> RevokeAll()
        {
            var status = await _service.RevokeAll();

            if (status)
            {
                return NotFound();
            }

            return Ok(status);
        }
    }
}
