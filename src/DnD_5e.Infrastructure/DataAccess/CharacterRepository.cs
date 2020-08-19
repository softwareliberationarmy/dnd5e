using System;
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
                return new Character(
                    new Ability(record.Strength, record.StrengthSaveProficiency),
                    new Ability(record.Dexterity, record.DexteritySaveProficiency),
                    new Ability(record.Constitution, record.ConstitutionSaveProficiency),
                    new Ability(record.Intelligence, record.IntelligenceSaveProficiency),
                    new Ability(record.Wisdom, record.WisdomSaveProficiency),
                    new Ability(record.Charisma, record.CharismaSaveProficiency)
                );
            }
        }
    }
}

//TODO: add unit tests for this logic so we can test out the mapping