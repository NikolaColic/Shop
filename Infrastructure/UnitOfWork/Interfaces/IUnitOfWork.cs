using Infrastructure.Repository.Interfaces;

namespace Infrastructure.UnitOfWork.Interfaces
{
    public interface IUnitOfWork<T> : IDisposable where T : class
    {
        public IRepository<T> Repository { get; set; }
        public Task Commit();
    }
}
