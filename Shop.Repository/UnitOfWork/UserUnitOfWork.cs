using Data.Entities;
using Shop.Repository.EF;
using Shop.Repository.Repository.Implementation;

namespace Shop.Repository.UnitOfWork
{
    public class UserUnitOfWork : UnitOfWork<User>
    {
        public UserUnitOfWork(ApplicationDbContext context) : base(context)
        {
            this.Repository = new UserRepository(context);
        }
    }
}
