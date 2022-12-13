namespace Infrastructure.Repository.Interfaces
{
    public interface IUserRepository<T> : IRepository<T> where T : class
    {
        Task<T> CheckUser(string username, string password);
    }
}
