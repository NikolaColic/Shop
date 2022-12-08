using Data.Entities;
using Infrastructure.Service.Interfaces;
using Infrastructure.UnitOfWork.Interfaces;

namespace Shop.Service.Implementations
{
    public class ArticleService : IGenericService<Article>
    {
        private readonly IUnitOfWork<Article> _unitOfWork;

        public ArticleService(IUnitOfWork<Article> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Article>> GetAll()
        {
            var articles = await _unitOfWork.Repository.GetAll();
            return articles;
        }

        public async Task<Article> GetById(int id)
        {
            var article = await _unitOfWork.Repository.GetById(id);
            return article;
        }

        public async Task<bool> Insert(Article entity)
        {
            var status = await _unitOfWork.Repository.Insert(entity);
            return status;
        }

        public async Task<bool> Update(Article entity)
        {
            var status = await _unitOfWork.Repository.Update(entity);
            return status;
        }
    }
}
