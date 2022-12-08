using Data.Entities;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shop.Repository.EF;

namespace Shop.Repository.Repository.Implementation
{
    internal class UserRepository : IRepository<Article>
    {
        public readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext context)
        {
            this._db = context;
        }

        public async Task<IEnumerable<Article>> GetAll()
        {
            var users = await _db.Article
                .ToListAsync();

            return users;
        }

        public async Task<Article> GetById(int id)
        {
            var user = await _db.Article
                .SingleOrDefaultAsync(e => e.Id == id);

            return user;
        }

        public Task<bool> Insert(Article entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Article entity)
        {
            throw new NotImplementedException();
        }
    }
}
