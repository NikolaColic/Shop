using Data.Entities;

namespace Shop.Service.Interfaces
{
    public interface IGrpcArticleService
    {
        Task<List<Article>> GetAll();
        Task<Article> GetById(int id);
        Task<List<Article>> Buy(List<int> keys);
    }
}
