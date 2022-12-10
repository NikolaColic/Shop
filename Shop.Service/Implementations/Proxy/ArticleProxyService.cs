
using Data.Entities;
using Infrastructure.AppConfig.Interfaces;
using Infrastructure.Service.Interfaces;
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
            var entity = await _articleService.Buy(key);

            if (entity == null)
            {
                return null;
            }

            _appCache.Remove($"{CacheKeysConstants.Article}{entity.Key}");
            _appCache.Add($"{CacheKeysConstants.Article}{entity.Key}", entity, DateTimeOffset.FromUnixTimeSeconds(_appConfig.CacheTime));
            return entity;
        }

        public async Task<IEnumerable<Article>> GetAll()
        {
            var articles = await _appCache.GetAsync<IEnumerable<Article>>(CacheKeysConstants.Articles); 

            if(articles != null && articles.Any())
            {
                return articles;
            }

            articles = await _articleService.GetAll();

            _appCache.Add(CacheKeysConstants.Articles, articles, DateTimeOffset.FromUnixTimeSeconds(_appConfig.CacheTime));

            return articles;
        }

        public async Task<Article> GetById(int id)
        {
            var article = await _appCache.GetAsync<Article>($"{CacheKeysConstants.Article}{id}");

            if (article != null)
            {
                return article;
            }

            article = await _articleService.GetById(id);

            _appCache.Add($"{CacheKeysConstants.Article}{id}", article, DateTimeOffset.FromUnixTimeSeconds(_appConfig.CacheTime));

            return article;
        }

        public async Task<Article> Insert(Article entity)
        {
            entity = await _articleService.Insert(entity);

            if (entity == null)
            {
                return null;
            }

            _appCache.Add($"{CacheKeysConstants.Article}{entity.Key}", entity, DateTimeOffset.FromUnixTimeSeconds(_appConfig.CacheTime));
            return entity;
        }

        public async Task<List<Article>> Order(List<int> keys)
        {
            var articles = await _articleService.Order(keys);

            if (articles == null)   
            {
                return null;
            }

            foreach (var article in articles)
            {
                _appCache.Add($"{CacheKeysConstants.Article}{article.Key}", articles, DateTimeOffset.FromUnixTimeSeconds(_appConfig.CacheTime));
            }

            return articles;
        }

        public async Task<Article> Update(Article entity)
        {
            entity = await _articleService.Update(entity);

            if (entity == null)
            {
                return null;
            }

            _appCache.Remove($"{CacheKeysConstants.Article}{entity.Key}");
            _appCache.Add($"{CacheKeysConstants.Article}{entity.Key}", entity, DateTimeOffset.FromUnixTimeSeconds(_appConfig.CacheTime));

            return entity;
        }
    }
}
