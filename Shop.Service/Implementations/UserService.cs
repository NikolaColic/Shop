using Data.Entities;
using Infrastructure.Service.Interfaces;
using Infrastructure.UnitOfWork.Interfaces;

namespace Shop.Service.Implementations
{
    public class UserService : IGenericService<User>
    {
        private readonly IUnitOfWork<User> _unitOfWork;

        public UserService(IUnitOfWork<User> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var articles = await _unitOfWork.Repository.GetAll();
            return articles;
        }

        public async Task<User> GetById(int id)
        {
            var article = await _unitOfWork.Repository.GetById(id);
            return article;
        }

        public async Task<bool> Insert(User entity)
        {
            var status = await _unitOfWork.Repository.Insert(entity);
            return status;
        }

        public async Task<bool> Update(User entity)
        {
            var status = await _unitOfWork.Repository.Update(entity);
            return status;
        }
    }
}
