using System.Collections.Generic;

namespace DnD_5e.Domain.CharacterRolls
{
    public class Skill
    {
        public enum Type
        {
            Acrobatics = 1,
            AnimalHandling,
            Arcana,
            Athletics,
            Deception,
            History,
            Insight,
            Intimidation,
            Investigation,
            Medicine,
            Nature,
            Perception,
            Performance,
            Persuasion,
            Religion,
            SleightOfHand,
            Stealth,
            Survival
        }
    }

    public static class SkillTypeExtensions
    {
        private static readonly Dictionary<Skill.Type, Ability.Type> _skillAbilityTable = 
            new Dictionary<Skill.Type, Ability.Type>
            {
                {Skill.Type.Acrobatics, Ability.Type.Dexterity},
                {Skill.Type.AnimalHandling, Ability.Type.Wisdom},
                {Skill.Type.Arcana, Ability.Type.Intelligence},
                {Skill.Type.Athletics, Ability.Type.Strength},
                {Skill.Type.Deception, Ability.Type.Charisma},
                {Skill.Type.History, Ability.Type.Intelligence},
                {Skill.Type.Insight, Ability.Type.Wisdom},
                {Skill.Type.Intimidation, Ability.Type.Charisma},
                {Skill.Type.Investigation, Ability.Type.Intelligence},
                {Skill.Type.Medicine, Ability.Type.Wisdom},
                {Skill.Type.Nature, Ability.Type.Intelligence},
                {Skill.Type.Perception, Ability.Type.Wisdom},
                {Skill.Type.Performance, Ability.Type.Charisma},
                {Skill.Type.Persuasion, Ability.Type.Charisma},
                {Skill.Type.Religion, Ability.Type.Intelligence},
                {Skill.Type.SleightOfHand, Ability.Type.Dexterity},
                {Skill.Type.Stealth, Ability.Type.Dexterity},
                {Skill.Type.Survival, Ability.Type.Wisdom}
            };

        public static Ability.Type GetParentAbility(this Skill.Type me)
        {
            return _skillAbilityTable[me];
        }
    }

}