using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Service.Interfaces.Proxy;

namespace Shop.Api.Controllers
{
    /// <summary>
    /// Article controller
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleProxyService _service;

        public ArticleController(IArticleProxyService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all articles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> Get()
        {
            var articles = await _service.GetAll();
            return Ok(articles);
        }

        /// <summary>
        /// Get article by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetById(int id)
        {
            var article = await _service.GetById(id);
            return Ok(article);
        }

        /// <summary>
        /// Customer buy article
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Policy = "Customer")]
        [HttpPost("{key}/buy")]
        public async Task<ActionResult<Article>> Buy(int id)
        {
            var article = await _service.Buy(id);
            return Ok(article);
        }

        /// <summary>
        /// Admin order articles from vendor
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        [Authorize(Policy = "Admin")]
        [HttpPost("order")]
        public async Task<ActionResult<Article>> Order([FromBody] List<int> keys)
        {
            var articles = await _service.Order(keys);
            return Ok(articles);
        }

    }
}
