using Data.Entities;
using Infrastructure.Exceptions;
using Infrastructure.Execution.Interfaces;
using Infrastructure.Repository.Interfaces;
using Infrastructure.UnitOfWork.Interfaces;
using Shop.Service.Interfaces;

namespace Shop.Service.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork<Article> _unitOfWork;
        private readonly IUserInfo userInfo;
        private readonly IGrpcArticleService _articleGrpcService;

        public ArticleService(IUnitOfWork<Article> unitOfWork, IUserInfo userInfo, IGrpcArticleService grpcService)
        {
            _unitOfWork = unitOfWork;
            this.userInfo = userInfo;
            _articleGrpcService = grpcService;
        }

        public async Task<Article> Buy(int key)
        {
            var article = await _unitOfWork.Repository.GetById(key);

            if (article == null)
            {
                var articles = await _articleGrpcService.Buy(new List<int>() { key });

                if (articles == null || !articles.Any())
                {
                    throw new EntityNotFoundException($"Article with key {key} doesn't exist");
                }

                article = articles.FirstOrDefault();
                SetBuyValues(article);

                await _unitOfWork.Repository.Insert(article);
            }
            else
            {
                SetBuyValues(article);
                await _unitOfWork.Repository.Update(article);
            }

            await _unitOfWork.Commit();

            return article;
        }

        private void SetBuyValues(Article article)
        {
            article.IsSold = true;
            article.UserId = userInfo.Id;
            article.SoldDate = DateTime.Now;
        }

        public async Task<IEnumerable<Article>> GetAll()
        {
            var articles = (await _unitOfWork.Repository.GetAll())
                .ToList();

            if (userInfo.IsAdmin)
            {
                var articlesVendor = await _articleGrpcService.GetAll();

                if (articlesVendor != null)
                {
                    articles.AddRange(articlesVendor);
                }
            }

            return articles.DistinctBy(e => e.Key);
        }

        public async Task<Article> GetById(int id)
        {
            var article = await _unitOfWork.Repository.GetById(id);

            if (article == null )
            {
                if (userInfo.IsAdmin)
                {
                    article = await _articleGrpcService.GetById(id);

                    if (article == null)
                    {
                        throw new EntityNotFoundException($"Article with key {id} doesn't exist");
                    }
                }
                else
                {
                    throw new EntityNotFoundException($"Article with key {id} doesn't exist");
                }
            }

            return article;
        }

        public async Task<Article> Insert(Article entity)
        {
            entity = await _unitOfWork.Repository.Insert(entity);
            await _unitOfWork.Commit();
            return entity;
        }

        public async Task<List<Article>> Order(List<int> keys)
        {
            var articles = await _articleGrpcService.Buy(keys);

            if (articles == null || !articles.Any())
            {
                throw new EntityNotFoundException($"Articles doesn't exist at vendor");
            }

            if (_unitOfWork.Repository is not IRepositoryList<Article> articleRepositoryList)
            {
                throw new Exception("Repository doesn't exist");
            }

            articles = await articleRepositoryList.AddRange(articles);
            await _unitOfWork.Commit();

            return articles;
        }

        public async Task<Article> Update(Article entity)
        {
            entity = await _unitOfWork.Repository.Update(entity);
            await _unitOfWork.Commit();

            return entity;
        }
    }
}
