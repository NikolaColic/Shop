namespace Infrastructure.Repository.Interfaces;
public interface IRepositoryList<T> : IRepository<T> where T : class
{
    Task<List<T>> AddRange(List<T> articles);
    Task<List<T>> GetByIds(List<int> ids);
}
