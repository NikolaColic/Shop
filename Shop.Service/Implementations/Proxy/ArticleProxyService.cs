
using Data.Entities;
using Infrastructure.AppConfig.Interfaces;
using Infrastructure.Service.Interfaces;
using LazyCache;
using Shop.Service.Constants;
using Shop.Service.Interfaces.Proxy;

namespace Shop.Service.Implementations.Proxy
{
    public class ArticleProxyService : IProxyService<Article>
    {
        private readonly IGenericService<Article> _articleService;
        private readonly IAppCache _appCache;
        private readonly IAppConfig _appConfig;

        public ArticleProxyService(IGenericService<Article> articleService, IAppCache appCache, IAppConfig appConfig)
        {
            _articleService = articleService;
            _appCache = appCache;
            _appConfig = appConfig;
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

        public async Task<bool> Insert(Article entity)
        {
            var status = await _articleService.Insert(entity);

            if (status)
            {
                _appCache.Add($"{CacheKeysConstants.Article}{entity.Key}", entity, DateTimeOffset.FromUnixTimeSeconds(_appConfig.CacheTime));
            }

            return status;
        }

        public async Task<bool> Update(Article entity)
        {
            var status = await _articleService.Update(entity);

            if (status)
            {
                _appCache.Remove($"{CacheKeysConstants.Article}{entity.Key}");
                _appCache.Add($"{CacheKeysConstants.Article}{entity.Key}", entity, DateTimeOffset.FromUnixTimeSeconds(_appConfig.CacheTime));
            }

            return status;
        }
    }
}
