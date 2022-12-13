using Data.Entities;
using Infrastructure.Exceptions;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shop.Repository.EF;

namespace Shop.Repository.Repository.Implementation
{
    public class UserRepository : IUserRepository<User>
    {
        public readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext context)
        {
            this._db = context;
        }

        public async Task<User> CheckUser(string username, string password)
        {
            var user = await this._db.User
                .FirstOrDefaultAsync(e => e.Username == username && e.Password == password);

            if (user is null)
            {
                throw new BadRequestEntityException("Bad username or password!");
            }

            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var users = await _db.User
                .ToListAsync();

            if (users is null)
            {
                throw new EntityNotFoundException($"{nameof(User)} doesn't exists");
            }

            return users;
        }

        public async Task<User> GetById(int id)
        {
            var user = await _db.User
                .SingleOrDefaultAsync(e => e.Id == id);

            if (user is null)
            {
                throw new EntityNotFoundException($"{nameof(Article)} doesn't exist");
            }

            return user;
        }

        public Task<User> Insert(User entity)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Update(User entity)
        {
            var userForUpdate = await GetById(entity.Id);

            _db.Entry(userForUpdate).State = EntityState.Detached;
            _db.Update(entity);

            return entity;
        }
    }
}
