using System.ComponentModel.DataAnnotations.Schema;
using DnD_5e.Domain.CharacterRolls;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DnD_5e.Infrastructure.DataAccess.Entities
{
    [Table("SkillProficiency")]
    public class SkillProficiencyEntity
    {
        public int Id { get; set; }
        public int Type { get; set; }

        internal static void Configure(EntityTypeBuilder<SkillProficiencyEntity> builder)
        {
            builder.Property(p => p.Type).IsRequired();
        }
    }
}