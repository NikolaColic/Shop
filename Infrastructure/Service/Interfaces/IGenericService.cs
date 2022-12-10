namespace Infrastructure.Service.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<T> GetById(int id);
        public Task<T> Insert(T entity);
        public Task<T> Update(T entity);
    }
}
