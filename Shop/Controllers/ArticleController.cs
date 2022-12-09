using Data.Entities;
using Infrastructure.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shop.Service.Interfaces.Proxy;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IProxyService<Article> _service;

        public ArticleController(IProxyService<Article> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> Get()
        {
            var articles = await _service.GetAll();

            if(articles == null)
            {
                return NotFound();
            }

            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetById(int id)
        {
            var article = await _service.GetById(id);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

    }
}
