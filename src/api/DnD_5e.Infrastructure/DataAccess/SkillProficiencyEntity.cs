using System.ComponentModel.DataAnnotations.Schema;

namespace DnD_5e.Infrastructure.DataAccess
{
    [Table("SkillProficiency")]
    public class SkillProficiencyEntity
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public CharacterEntity Character { get; set; }
    }
}