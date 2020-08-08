using System;
using System.Collections.Generic;

namespace DnD_5e.Domain.Roleplay
{
    public class Character
    {
        private readonly Dictionary<string, Func<Character, Ability>> _abilities =
            new Dictionary<string, Func<Character, Ability>>
            {
                {"strength", c => c.Strength},
                {"dexterity", c => c.Dexterity},
                {"constitution", c => c.Constitution},
                {"intelligence", c => c.Intelligence},
                {"wisdom", c => c.Wisdom},
                {"charisma", c => c.Charisma}
            };

        public Ability Strength { get; set; }
        public Ability Dexterity { get; set; }
        public Ability Constitution { get; set; }
        public Ability Intelligence { get; set; }
        public Ability Wisdom { get; set; }
        public Ability Charisma { get; set; }

        public int Proficiency { get; } = 2;

        public string GetAbilityRoll(string abilityName)
        {
            var modifier = GetAbility(abilityName).GetAbilityModifier();
            return "1d20" + (modifier > 0 ? "+" + modifier : modifier < 0 ? "-" + modifier : "");
        }

        public string GetSavingThrow(string abilityName)
        {
            var ability = GetAbility(abilityName);
            var modifier = ability.GetAbilityModifier();
            if (ability.ProficientAtSaves)
            {
                modifier += this.Proficiency;
            }
            return "1d20" + (modifier > 0 ? "+" + modifier : modifier < 0 ? "-" + modifier : "");
        }

        private Ability GetAbility(string ability)
        {
            var input = ability.ToLower().Trim();
            if (_abilities.ContainsKey(input))
            {
                return _abilities[input](this);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(ability));
            }
        }
    }
}
