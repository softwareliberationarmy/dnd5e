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
                    Strength = new Ability(record.Strength, record.StrengthSaveProficiency),
                    Dexterity = new Ability(record.Dexterity,record.DexteritySaveProficiency),
                    Constitution = new Ability(record.Constitution, record.ConstitutionSaveProficiency),
                    Intelligence = new Ability(record.Intelligence, record.IntelligenceSaveProficiency),
                    Wisdom = new Ability(record.Wisdom, record.WisdomSaveProficiency),
                    Charisma = new Ability(record.Charisma, record.CharismaSaveProficiency)
                };
            }
        }
    }
}
