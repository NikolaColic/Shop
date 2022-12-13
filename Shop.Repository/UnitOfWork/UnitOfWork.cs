using Infrastructure.Repository.Interfaces;
using Infrastructure.UnitOfWork.Interfaces;
using Shop.Repository.EF;

namespace Shop.Repository.UnitOfWork
{
    public abstract class UnitOfWork<T> : IUnitOfWork<T> where T : class
    {
        public readonly ApplicationDbContext context;
        public IRepository<T> Repository { get; set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task Commit()
        {
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
