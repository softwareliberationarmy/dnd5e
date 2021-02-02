using System.Collections.Generic;
using DnD_5e.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DnD_5e.Infrastructure.DataAccess.Entities
{
    public class CharacterEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
        public bool StrengthSaveProficiency { get; set; }
        public bool DexteritySaveProficiency { get; set; }
        public bool ConstitutionSaveProficiency { get; set; }
        public bool IntelligenceSaveProficiency { get; set; }
        public bool WisdomSaveProficiency { get; set; }
        public bool CharismaSaveProficiency { get; set; }

        public Race Race { get; set; }
        public Class Class { get; set; }

        public int ExperiencePoints { get; set; }

        public List<SkillProficiencyEntity> SkillProficiencies { get; set; }

        public UserEntity Owner { get; set; }

        internal static void Configure(EntityTypeBuilder<CharacterEntity> builder)
        {
            builder.HasMany(c => c.SkillProficiencies).WithOne().HasForeignKey("CharacterId");
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.HasOne(c => c.Owner).WithMany().IsRequired();
            builder.Property(c => c.Race).HasConversion<int>(c => (int)c, c => (Race)c);
            builder.Property(c => c.Class).HasConversion<int>(c => (int)c, c => (Class)c);
        }
    }
}