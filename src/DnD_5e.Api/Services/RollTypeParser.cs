using System;
using DnD_5e.Domain.Roleplay;

namespace DnD_5e.Api.Services
{
    public class RollTypeParser
    {
        public CharacterRollRequest ParseRequest(string input, bool isSave = false)
        {
            var ability = input.ToLower().Trim();
            if (Enum.TryParse(input, true, out Ability.Type abilityType))
            {
                return new CharacterRollRequest(abilityType, isSave);
            }
            else if (Enum.TryParse(input.Replace(" ", ""), true, out Skill.Type skillType))
            {
                return new CharacterRollRequest(skillType, skillType.GetParentAbility());
            }
            throw new ArgumentOutOfRangeException(nameof(input));
        }
    }
}