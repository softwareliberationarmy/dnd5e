using System;
using System.Collections.Generic;
using System.Text;

namespace DnD_5e.Domain.Roleplay
{
    public class Character
    {
        private Dictionary<string,Func<Character,int>> _abilities = new Dictionary<string, Func<Character, int>>
        {
            {"strength", c => c.Strength},
            {"dexterity", c => c.Dexterity},
            {"constitution", c => c.Constitution},
            {"intelligence", c => c.Intelligence},
            {"wisdom", c => c.Wisdom},
            {"charisma", c => c.Charisma}
        };

        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }

        public string GetAbilityRoll(string ability)
        {
            var attribute = GetAttribute(ability);
            var modifier = (attribute - 10) / 2;
            var roll = "1d20" + (modifier > 0 ? "+" + modifier : modifier < 0 ? "-" + modifier : "");
            return roll;
        }

        private int GetAttribute(string ability)
        {
            var input = ability.ToLower().Trim();
            if (_abilities.ContainsKey(input))
            {
                return _abilities[input](this);
            }
            //TODO: add a unit test
            //else
            //{
            //    throw new ArgumentOutOfRangeException(nameof(ability));
            //}
        }
    }
}
