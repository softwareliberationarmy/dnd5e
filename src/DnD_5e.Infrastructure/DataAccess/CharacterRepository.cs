using System.Linq;
using DnD_5e.Domain.Roleplay;

namespace DnD_5e.Infrastructure.DataAccess
{
    public class CharacterRepository
    {
        private readonly CharacterDbContext _context;

        public CharacterRepository(CharacterDbContext context)
        {
            _context = context;
        }

        public Character GetById(int id)
        {
            var record = _context.Characters.FirstOrDefault(c => c.Id == id);
            if (record == null)
            {
                return null;
            }
            else
            {
                return new Character
                {
                    Strength = new Ability(record.Strength, true),
                    Dexterity = record.Dexterity,
                    Constitution = record.Constitution,
                    Intelligence = record.Intelligence,
                    Wisdom = record.Wisdom,
                    Charisma = record.Charisma
                };
            }
        }
    }
}
