namespace DnD_5e.Domain.Roleplay
{
    public class CharacterRollRequest
    {
        public CharacterRollRequest(Ability.Type abilityType, bool isSave = false)
        {
            RollType = isSave ? RollTypeEnum.AbilitySavingThrow : RollTypeEnum.AbilityCheck;
            AbilityType = abilityType;
            IsSavingThrow = isSave;
        }

        public CharacterRollRequest(Skill.Type skillType, Ability.Type abilityType)
        {
            RollType = RollTypeEnum.SkillCheck;
            SkillType = skillType;
            AbilityType = abilityType;
        }

        public CharacterRollRequest(RollTypeEnum rollType)
        {
            RollType = rollType;
        }

        public Ability.Type AbilityType { get; }
        public Skill.Type SkillType { get; }
        public bool IsSavingThrow { get; }
        public RollTypeEnum RollType { get; set; }
    }
}