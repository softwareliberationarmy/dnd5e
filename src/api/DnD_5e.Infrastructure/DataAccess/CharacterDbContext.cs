using Microsoft.EntityFrameworkCore;

namespace DnD_5e.Infrastructure.DataAccess
{
    public class CharacterDbContext : DbContext
    {
        public CharacterDbContext(DbContextOptions<CharacterDbContext> options) :
            base(options)
        {
        }

        public DbSet<CharacterEntity> Character { get; set; }
        public DbSet<UserEntity> User { get; set; }
    }
}