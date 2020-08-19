using System.Collections;
using System.Collections.Generic;
using DnD_5e.Domain.Roleplay;

namespace DnD_5e.Infrastructure.DataAccess
{
    public class CharacterEntity
    {
        public virtual int Id { get; set; }
        public virtual int Strength { get; set; }
        public virtual int Dexterity { get; set; }
        public virtual int Constitution { get; set; }
        public virtual int Intelligence { get; set; }
        public virtual int Wisdom { get; set; }
        public virtual int Charisma { get; set; }
        public virtual bool StrengthSaveProficiency { get; set; }
        public virtual bool DexteritySaveProficiency { get; set; }
        public virtual bool ConstitutionSaveProficiency { get; set; }
        public virtual bool IntelligenceSaveProficiency { get; set; }
        public virtual bool WisdomSaveProficiency { get; set; }
        public virtual bool CharismaSaveProficiency { get; set; }

        public virtual ICollection<SkillProficiencyEntity> SkillProficiencies { get; set; }
    }
}