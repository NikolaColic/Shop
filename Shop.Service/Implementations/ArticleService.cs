using Data.Entities;
using Infrastructure.Execution.Interfaces;
using Infrastructure.Repository.Interfaces;
using Infrastructure.Service.Interfaces;
using Infrastructure.UnitOfWork.Interfaces;
using Shop.Service.Interfaces;

namespace Shop.Service.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork<Article> _unitOfWork;
        private readonly IUserInfo userInfo;

        public ArticleService(IUnitOfWork<Article> unitOfWork, IUserInfo userInfo)
        {
            _unitOfWork = unitOfWork;
            this.userInfo = userInfo;
        }

        public async Task<Article> Buy(int key)
        {
            var article = await _unitOfWork.Repository.GetById(key);
            
            if(article == null)
            {
                //poziva se vendor
            }

            article.IsSold = true;
            article.UserId = userInfo.Id;
            article.SoldDate = DateTime.Now;

            await _unitOfWork.Repository.Update(article);
            await _unitOfWork.Commit();

            return article;
        }

        public async Task<IEnumerable<Article>> GetAll()
        {
            var articles = await _unitOfWork.Repository.GetAll();
            return articles;
        }

        public async Task<Article> GetById(int id)
        {
            var article = await _unitOfWork.Repository.GetById(id);
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
            //Zove se vendor;
            var articles = new List<Article>();

            if(articles == null)
            {

            }

            var articleRepositoryList = _unitOfWork.Repository as IRepositoryList<Article>;

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
