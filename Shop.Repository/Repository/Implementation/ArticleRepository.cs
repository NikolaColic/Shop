using Data.Entities;
using Infrastructure.Exceptions;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shop.Repository.EF;

namespace Shop.Repository.Repository.Implementation
{
    public class ArticleRepository : IRepositoryList<Article>
    {
        public readonly ApplicationDbContext _db;
        public ArticleRepository(ApplicationDbContext context)
        {
            this._db = context;
        }

        public async Task<List<Article>> AddRange(List<Article> articles)
        {
            await _db.AddRangeAsync(articles);
            return articles;
        }

        public async Task<IEnumerable<Article>> GetAll()
        {
            var articles = await _db.Article
                .Where(e => !e.IsSold)
                .ToListAsync();

            if (articles is null)
            {
                throw new EntityNotFoundException($"{nameof(Article)} doesn't exists");
            }

            return articles;
        }

        public async Task<Article> GetById(int id)
        {
            var article = await _db.Article
                .FirstOrDefaultAsync(e => e.Key == id && !e.IsSold);

            return article;
        }

        public Task<List<Article>> GetByIds(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public async Task<Article> Insert(Article entity)
        {
            await _db.AddAsync(entity);
            return entity;
        }

        public async Task<Article> Update(Article entity)
        {
            var articleForUpdate = await GetById(entity.Id);

            _db.Entry(articleForUpdate).State = EntityState.Detached;
            _db.Update(entity);

            return entity;
        }
    }
}
