﻿using System.Collections.Generic;

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
        
        public int ExperiencePoints { get; set; }

        public List<SkillProficiencyEntity> SkillProficiencies { get; set; }

        public UserEntity Owner { get; set; }
    }
}