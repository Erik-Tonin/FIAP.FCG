using FIAP.FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FIAP.FCG.Infra.Context
{
    public class MicroServiceContext : DbContext
    {
        public MicroServiceContext(DbContextOptions<MicroServiceContext> options) : base(options)
        {

        }

        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<Game> Game { get; set; }
        public DbSet<Category> Category { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MicroServiceContext).Assembly);
        }
    }
}
