namespace Infrastructure.Service.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<T> GetById(int id);
        public Task<bool> Insert(T entity);
        public Task<bool> Update(T entity);
    }
}
