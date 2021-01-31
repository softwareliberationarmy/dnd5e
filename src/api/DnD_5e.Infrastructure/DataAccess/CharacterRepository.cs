using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnD_5e.Domain.CharacterRolls;
using DnD_5e.Infrastructure.DataAccess.Mapping;
using DnD_5e.Infrastructure.DataAccess.Pocos;
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

        public Character GetById(int id)
        {
            return _context.Character.Include(c => c.SkillProficiencies).Where(c => c.Id == id)
                .MapToCharacter()
                .FirstOrDefault();
        }

        public IEnumerable<CharacterListPoco> GetByOwner(string userName)
        {
            return _context.Character.Where(c => c.Owner.Name == userName).MapToListCharacter();
        }
    }
}