using Data.Entities;
using Vendor.Api.Repository.EF;

namespace Vendor.Api.Repository.UnitOfWork
{
    public class ArticleUnitOfWork : UnitOfWork<Article>
    {
        public ArticleUnitOfWork(ApplicationDbContext context) : base(context)
        {
            this.Repository = new ArticleRepository(context);
        }
    }
}
