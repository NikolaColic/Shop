using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Vendor.Api.Repository.EF
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Article> Article { get; set; }
        public DbSet<User> User { get; set; }
    }
}
