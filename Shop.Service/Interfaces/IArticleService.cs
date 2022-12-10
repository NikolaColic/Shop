using Data.Entities;
using Infrastructure.Service.Interfaces;

namespace Shop.Service.Interfaces
{
    public interface IArticleService : IGenericService<Article>
    {
        Task<Article> Buy(int key);
        Task<List<Article>> Order(List<int> keys);
    }
}
