namespace DnD_5e.Domain.CharacterRolls
{
    public class Ability
    {
        private readonly int _score;

        public Ability(int score, bool proficientAtSaves)
        {
            _score = score;
            ProficientAtSaves = proficientAtSaves;
        }

        public bool ProficientAtSaves { get; }

        public int GetAbilityModifier()
        {
            return (_score - 10) / 2;
        }

        public enum Type
        {
            Strength = 1,
            Dexterity,
            Constitution,
            Intelligence,
            Wisdom,
            Charisma
        }
    }
}
