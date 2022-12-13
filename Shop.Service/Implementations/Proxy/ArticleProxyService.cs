
using Data.Entities;
using Infrastructure.AppConfig.Interfaces;
using LazyCache;
using Shop.Service.Constants;
using Shop.Service.Interfaces;
using Shop.Service.Interfaces.Proxy;

namespace Shop.Service.Implementations.Proxy
{
    public class ArticleProxyService : IArticleProxyService
    {
        private readonly IArticleService _articleService;
        private readonly IAppCache _appCache;
        private readonly IAppConfig _appConfig;

        public ArticleProxyService(IArticleService articleService, IAppCache appCache, IAppConfig appConfig)
        {
            _articleService = articleService;
            _appCache = appCache;
            _appConfig = appConfig;
        }

        public async Task<Article> Buy(int key)
        {
            var article = await _articleService.Buy(key);

            _appCache.Remove($"{CacheKeyConstants.Article}{article.Key}");
            _appCache.Add($"{CacheKeyConstants.Article}{article.Key}", article, DateTimeOffset.UtcNow.AddSeconds(_appConfig.CacheTime));
            return article;
        }

        public async Task<IEnumerable<Article>> GetAll()
        {
            var articles = await _appCache.GetAsync<IEnumerable<Article>>(CacheKeyConstants.Articles);

            if (articles != null && articles.Any())
            {
                return articles;
            }

            articles = await _articleService.GetAll();

            _appCache.Add(CacheKeyConstants.Articles, articles, DateTimeOffset.UtcNow.AddSeconds(_appConfig.CacheTime));

            return articles;
        }

        public async Task<Article> GetById(int id)
        {
            var article = await _appCache.GetAsync<Article>($"{CacheKeyConstants.Article}{id}");

            if (article != null)
            {
                return article;
            }

            article = await _articleService.GetById(id);

            _appCache.Add($"{CacheKeyConstants.Article}{id}", article, DateTimeOffset.UtcNow.AddSeconds(_appConfig.CacheTime));

            return article;
        }

        public async Task<Article> Insert(Article entity)
        {
            entity = await _articleService.Insert(entity);
            _appCache.Add($"{CacheKeyConstants.Article}{entity.Key}", entity, DateTimeOffset.UtcNow.AddSeconds(_appConfig.CacheTime));

            return entity;
        }

        public async Task<List<Article>> Order(List<int> keys)
        {
            var articles = await _articleService.Order(keys);

            foreach (var article in articles)
            {
                _appCache.Add($"{CacheKeyConstants.Article}{article.Key}", articles, DateTimeOffset.UtcNow.AddSeconds(_appConfig.CacheTime));
            }

            return articles;
        }

        public async Task<Article> Update(Article entity)
        {
            entity = await _articleService.Update(entity);

            _appCache.Remove($"{CacheKeyConstants.Article}{entity.Key}");
            _appCache.Add($"{CacheKeyConstants.Article}{entity.Key}", entity, DateTimeOffset.UtcNow.AddSeconds(_appConfig.CacheTime));

            return entity;
        }
    }
}
