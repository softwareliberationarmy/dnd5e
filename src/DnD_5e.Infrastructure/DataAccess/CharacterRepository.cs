using System;
using System.Linq;
using System.Threading.Tasks;
using DnD_5e.Domain.Roleplay;
using Microsoft.EntityFrameworkCore;

namespace DnD_5e.Infrastructure.DataAccess
{
    public class CharacterRepository
    {
        private readonly CharacterDbContext _context;

        public CharacterRepository(CharacterDbContext context)
        {
            _context = context;
        }

        public async Task<Character> GetById(int id)
        {
            var record = await _context.Characters.Include(c => c.SkillProficiencies)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (record == null)
            {
                return null;
            }
            else
            {
                var proficiencies = record.SkillProficiencies.Select(s =>
                    (Skill.Type) s.Type);
                return new Character(
                    new Ability(record.Strength, record.StrengthSaveProficiency),
                    new Ability(record.Dexterity, record.DexteritySaveProficiency),
                    new Ability(record.Constitution, record.ConstitutionSaveProficiency),
                    new Ability(record.Intelligence, record.IntelligenceSaveProficiency),
                    new Ability(record.Wisdom, record.WisdomSaveProficiency),
                    new Ability(record.Charisma, record.CharismaSaveProficiency),
                    proficiencies.ToArray(), record.ExperiencePoints
                );
            }
        }
    }
}