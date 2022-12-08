using Data.Entities;
using Shop.Repository.EF;
using Shop.Repository.Repository.Implementation;

namespace Shop.Repository.UnitOfWork
{
    public class ArticleUnitOfWork : UnitOfWork<Article>
    {
        public ArticleUnitOfWork(ApplicationDbContext context) : base(context)
        {
            this.Repository = new ArticleRepository(context);
        }
    }
}
