namespace DnD_5e.Domain.Roleplay
{
    public class CharacterRollRequest
    {
        public CharacterRollRequest(Ability.Type abilityType)
        {
            AbilityType = abilityType;
        }

        public CharacterRollRequest(Skill.Type skillType, Ability.Type abilityType)
        {
            SkillType = skillType;
            AbilityType = abilityType;
        }

        public Ability.Type AbilityType { get; }
        public Skill.Type SkillType { get; }
    }
}