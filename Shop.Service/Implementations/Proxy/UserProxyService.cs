using Data.Entities;
using Infrastructure.AppConfig.Interfaces;
using Infrastructure.Service.Interfaces;
using LazyCache;
using Shop.Service.Constants;
using Shop.Service.Interfaces.Proxy;

namespace Shop.Service.Implementations.Proxy
{
    public class UserProxyService : IProxyService<User>
    {
        private readonly IGenericService<User> _articleService;
        private readonly IAppCache _appCache;
        private readonly IAppConfig _appConfig;

        public UserProxyService(IGenericService<User> articleService, IAppCache appCache, IAppConfig appConfig)
        {
            _articleService = articleService;
            _appCache = appCache;
            _appConfig = appConfig;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var articles = await _appCache.GetAsync<IEnumerable<User>>(CacheKeysConstants.Users);

            if (articles != null && articles.Any())
            {
                return articles;
            }

            articles = await _articleService.GetAll();

            _appCache.Add(CacheKeysConstants.Users, articles, DateTimeOffset.FromUnixTimeSeconds(_appConfig.CacheTime));

            return articles;
        }

        public async Task<User> GetById(int id)
        {
            var article = await _appCache.GetAsync<User>($"{CacheKeysConstants.User}{id}");

            if (article != null)
            {
                return article;
            }

            article = await _articleService.GetById(id);

            _appCache.Add($"{CacheKeysConstants.User}{id}", article, DateTimeOffset.FromUnixTimeSeconds(_appConfig.CacheTime));

            return article;
        }

        public async Task<User> Insert(User entity)
        {
            entity = await _articleService.Insert(entity);

            if (entity == null)
            {
                return null;
            }

            _appCache.Add($"{CacheKeysConstants.User}{entity.Id}", entity, DateTimeOffset.FromUnixTimeSeconds(_appConfig.CacheTime));

            return entity;
        }

        public async Task<User> Update(User entity)
        {
            entity = await _articleService.Update(entity);

            if (entity == null)
            {
                return null;
            }

            _appCache.Remove($"{CacheKeysConstants.User}{entity.Id}");
            _appCache.Add($"{CacheKeysConstants.User}{entity.Id}", entity, DateTimeOffset.FromUnixTimeSeconds(_appConfig.CacheTime));
            
            return entity;
        }
    }
}
