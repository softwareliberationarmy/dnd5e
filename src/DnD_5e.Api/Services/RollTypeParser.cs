using System;
using DnD_5e.Domain.Roleplay;

namespace DnD_5e.Api.Services
{
    public class RollTypeParser
    {
        public CharacterRollRequest ParseRequest(string input, bool isSave = false)
        {
            if (Enum.TryParse(input, true, out Ability.Type abilityType))
            {
                return new CharacterRollRequest(abilityType, isSave);
            }
            else if (Enum.TryParse(input.Replace(" ", ""), true, out Skill.Type skillType))
            {
                return new CharacterRollRequest(skillType, skillType.GetParentAbility());
            }
            else if (Enum.TryParse(input, true, out RollTypeEnum rollType))
            {
                return new CharacterRollRequest(rollType);
            }
            throw new ArgumentOutOfRangeException(nameof(input));
        }
    }
}