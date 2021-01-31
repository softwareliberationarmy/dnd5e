using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DnD_5e.Infrastructure.DataAccess.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        internal static void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
        }
    }

}