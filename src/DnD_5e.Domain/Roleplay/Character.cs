using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DnD_5e.Domain.Roleplay
{
    public class Character
    {
        private readonly Skill.Type[] _skillProficiencies;
        private readonly Dictionary<Ability.Type, Ability> _abilityDictionary;
        private readonly int _proficiency = 2;
        private static readonly int[] _xpProficiencyBumps = new[] {6500, 48000, 120000, 225000};

        public Character(Ability strength, Ability dexterity, Ability constitution,
            Ability intelligence, Ability wisdom, Ability charisma, Skill.Type[] skillProficiencies, 
            int experiencePoints = 0)
        {
            //TODO: remove this default value and make it required to build a character
            _skillProficiencies = skillProficiencies ?? new Skill.Type[0];
            _abilityDictionary = new Dictionary<Ability.Type, Ability>
            {
                {Ability.Type.Strength, strength},
                {Ability.Type.Dexterity, dexterity},
                {Ability.Type.Constitution, constitution},
                {Ability.Type.Intelligence, intelligence},
                {Ability.Type.Wisdom, wisdom},
                {Ability.Type.Charisma, charisma}
            };
            for (int i = 0; i < _xpProficiencyBumps.Length; i++)
            {
                if (experiencePoints >= _xpProficiencyBumps[i])
                {
                    _proficiency++;
                }
            }
        }

        public string GetRoll(CharacterRollRequest characterRollRequest)
        {
            var ability = _abilityDictionary[characterRollRequest.AbilityType];
            var abilityModifier = ability.GetAbilityModifier();
            if (characterRollRequest.IsSavingThrow && ability.ProficientAtSaves)
            {
                abilityModifier += _proficiency;
            }
            else if (characterRollRequest.SkillType != default(Skill.Type) &&
                     _skillProficiencies.Contains(characterRollRequest.SkillType))
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
