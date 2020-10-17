namespace DnD_5e.Infrastructure.DataAccess
{
    public class SkillProficiencyEntity
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public CharacterEntity Character { get; set; }
    }
}