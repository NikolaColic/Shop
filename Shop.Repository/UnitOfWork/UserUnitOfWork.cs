using Data.Entities;
using Shop.Repository.EF;
using Shop.Repository.Repository.Implementation;

namespace Shop.Repository.UnitOfWork
{
    public class UserUnitOfWork : UnitOfWork<Article>
    {
        public UserUnitOfWork(ApplicationDbContext context) : base(context)
        {
            this.Repository = new ArticleRepository(context);
        }
    }
}
