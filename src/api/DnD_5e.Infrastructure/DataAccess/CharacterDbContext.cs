using DnD_5e.Infrastructure.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DnD_5e.Infrastructure.DataAccess
{
    public class CharacterDbContext : DbContext
    {
        public CharacterDbContext(DbContextOptions<CharacterDbContext> options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity>(UserEntity.Configure);
            modelBuilder.Entity<SkillProficiencyEntity>(SkillProficiencyEntity.Configure);
            modelBuilder.Entity<CharacterEntity>(CharacterEntity.Configure);
        }

        public DbSet<CharacterEntity> Character { get; set; }
        public DbSet<UserEntity> User { get; set; }
    }
}