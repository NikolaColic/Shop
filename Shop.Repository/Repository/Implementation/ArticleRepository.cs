using Data.Entities;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shop.Repository.EF;

namespace Shop.Repository.Repository.Implementation
{
    internal class ArticleRepository : IRepository<Article>
    {
        public readonly ApplicationDbContext _db;
        public ArticleRepository(ApplicationDbContext context)
        {
            this._db = context;
        }

        public async Task<IEnumerable<Article>> GetAll()
        {
            var articles = await _db.Article
                .ToListAsync();

            return articles;
        }

        public async Task<Article> GetById(int id)
        {
            var article = await _db.Article
                .SingleOrDefaultAsync(e => e.Id == id);

            return article;
        }

        public async Task<bool> Insert(Article entity)
        {
            if(entity.User != null)
            {
                var user = await _db.User
                    .SingleOrDefaultAsync(e => e.Id == entity.User.Id);

                if(user == null)
                {

                }

                entity.User = user;
            }

            await _db.AddAsync(entity);
            return true;
        }

        public async Task<bool> Update(Article entity)
        {
            var articleForUpdate = await GetById(entity.Id);

            if(articleForUpdate == null)
            {

            }

            _db.Entry(articleForUpdate).State = EntityState.Detached;
            _db.Update(entity);

            return true;
        }
    }
}
