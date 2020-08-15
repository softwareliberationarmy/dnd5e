namespace DnD_5e.Domain.Roleplay
{
    public class CharacterRollRequest
    {
        public CharacterRollRequest(Ability.Type abilityType)
        {
            AbilityType = abilityType;
        }

        public Ability.Type AbilityType { get; }
    }
}