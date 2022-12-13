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
            var articles = await _appCache.GetAsync<IEnumerable<User>>(CacheKeyConstants.Users);

            if (articles != null && articles.Any())
            {
                return articles;
            }

            articles = await _articleService.GetAll();

            _appCache.Add(CacheKeyConstants.Users, articles);

            return articles;
        }

        public async Task<User> GetById(int id)
        {
            var article = await _appCache.GetAsync<User>($"{CacheKeyConstants.User}{id}");

            if (article != null)
            {
                return article;
            }

            article = await _articleService.GetById(id);

            _appCache.Add($"{CacheKeyConstants.User}{id}", article, DateTimeOffset.UtcNow.AddSeconds(_appConfig.CacheTime));

            return article;
        }

        public async Task<User> Insert(User user)
        {
            user = await _articleService.Insert(user);

            _appCache.Add($"{CacheKeyConstants.User}{user.Id}", user, DateTimeOffset.UtcNow.AddSeconds(_appConfig.CacheTime));

            return user;
        }

        public async Task<User> Update(User user)
        {
            user = await _articleService.Update(user);

            _appCache.Remove($"{CacheKeyConstants.User}{user.Id}");
            _appCache.Add($"{CacheKeyConstants.User}{user.Id}", user, DateTimeOffset.UtcNow.AddSeconds(_appConfig.CacheTime));

            return user;
        }
    }
}
