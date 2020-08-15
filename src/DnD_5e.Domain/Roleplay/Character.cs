using System.Collections.Generic;

namespace DnD_5e.Domain.Roleplay
{
    public class Character
    {
        private readonly Dictionary<Ability.Type, Ability> _abilityDictionary;
        private readonly int _proficiency = 2;

        public Character(Ability strength, Ability dexterity, Ability constitution,
            Ability intelligence, Ability wisdom, Ability charisma)
        {
            _abilityDictionary = new Dictionary<Ability.Type, Ability>
            {
                {Ability.Type.Strength, strength},
                {Ability.Type.Dexterity, dexterity},
                {Ability.Type.Constitution, constitution},
                {Ability.Type.Intelligence, intelligence},
                {Ability.Type.Wisdom, wisdom},
                {Ability.Type.Charisma, charisma}
            };
        }

        public string GetRoll(CharacterRollRequest characterRollRequest)
        {
            var ability = _abilityDictionary[characterRollRequest.AbilityType];
            var abilityModifier = ability.GetAbilityModifier();
            if (characterRollRequest.IsSavingThrow && ability.ProficientAtSaves)
            {
                abilityModifier += _proficiency;
            }
            return D20RollWithModifier(abilityModifier);
        }

        private static string D20RollWithModifier(int modifier)
        {
            return "1d20" + (modifier > 0 ? "+" + modifier : modifier < 0 ? "-" + modifier : "");
        }
    }
}
