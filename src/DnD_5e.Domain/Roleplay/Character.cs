using System;
using System.Collections.Generic;

namespace DnD_5e.Domain.Roleplay
{
    public class Character
    {
        private readonly Dictionary<string,Func<Character,int>> _abilityScores = 
            new Dictionary<string, Func<Character, int>>
        {
            {"strength", c => c.Strength.Score},
            {"dexterity", c => c.Dexterity},
            {"constitution", c => c.Constitution},
            {"intelligence", c => c.Intelligence},
            {"wisdom", c => c.Wisdom},
            {"charisma", c => c.Charisma}
        };
        private readonly Dictionary<string, Func<Character, Ability>> _abilities =
            new Dictionary<string, Func<Character, Ability>>
            {
                {"strength", c => c.Strength},
                {"dexterity", c => new Ability(c.Dexterity, false)},
                {"constitution", c => new Ability(c.Constitution, false)},
                {"intelligence", c => new Ability(c.Intelligence, false)},
                {"wisdom", c => new Ability(c.Wisdom, false)},
                {"charisma", c => new Ability(c.Charisma, false)}
            };


        public Ability Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }

        public int Proficiency { get; } = 2;

        public string GetAbilityRoll(string abilityName)
        {
            var abilityScore = GetAbility(abilityName).Score;
            var modifier = (abilityScore - 10) / 2;
            return "1d20" + (modifier > 0 ? "+" + modifier : modifier < 0 ? "-" + modifier : "");
        }

        public string GetSavingThrow(string abilityName)
        {
            var ability = GetAbility(abilityName);
            var modifier = (ability.Score - 10) / 2;
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
