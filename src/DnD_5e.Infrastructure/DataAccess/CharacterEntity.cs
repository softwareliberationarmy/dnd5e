using System.Collections;
using System.Collections.Generic;
using DnD_5e.Domain.Roleplay;

namespace DnD_5e.Infrastructure.DataAccess
{
    public class CharacterEntity
    {
        public int Id { get; set; }
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

        public List<SkillProficiencyEntity> SkillProficiencies { get; set; }
        
        public int ExperiencePoints { get; set; }
    }
}