using System;
using System.Collections.Generic;

namespace DnD_5e.Domain.Roleplay
{
    public class Character
    {
        private readonly Dictionary<string, Func<Character, Ability>> _abilities =
            new Dictionary<string, Func<Character, Ability>>
            {
                {"strength", c => c._strength},
                {"dexterity", c => c._dexterity},
                {"constitution", c => c._constitution},
                {"intelligence", c => c._intelligence},
                {"wisdom", c => c._wisdom},
                {"charisma", c => c._charisma}
            };
        private readonly Ability _strength;
        private readonly Ability _dexterity;
        private readonly Ability _constitution;
        private readonly Ability _intelligence;
        private readonly Ability _wisdom;
        private readonly Ability _charisma;
        private readonly int _proficiency = 2;

        public Character(Ability strength, Ability dexterity, Ability constitution,
            Ability intelligence, Ability wisdom, Ability charisma)
        {
            _strength = strength;
            _dexterity = dexterity;
            _constitution = constitution;
            _intelligence = intelligence;
            _wisdom = wisdom;
            _charisma = charisma;
        }

        public string GetAbilityRoll(string abilityName)
        {
            var modifier = GetAbility(abilityName).GetAbilityModifier();
            return D20RollWithModifier(modifier);
        }

        public string GetSavingThrow(string abilityName)
        {
            var ability = GetAbility(abilityName);
            var modifier = ability.GetAbilityModifier();
            if (ability.ProficientAtSaves)
            {
                modifier += _proficiency;
            }
            return D20RollWithModifier(modifier);
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

        private static string D20RollWithModifier(int modifier)
        {
            return "1d20" + (modifier > 0 ? "+" + modifier : modifier < 0 ? "-" + modifier : "");
        }
    }
}
