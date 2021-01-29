using System.Collections.Generic;
using System.Linq;
using DnD_5e.Domain.CharacterRolls;
using DnD_5e.Infrastructure.DataAccess.Pocos;

namespace DnD_5e.Infrastructure.DataAccess.Mapping
{
    public static class CharacterMapping
    {
        public static IEnumerable<Character> MapToCharacter(this IQueryable<CharacterEntity> entities)
        {
            return entities.Select(record =>
                new
                {
                    Proficiencies = record.SkillProficiencies.Select(s => s.Type),
                    Character = record
                }).Select(c =>
                new Character(
                    new Ability(c.Character.Strength, c.Character.StrengthSaveProficiency),
                    new Ability(c.Character.Dexterity, c.Character.DexteritySaveProficiency),
                    new Ability(c.Character.Constitution, c.Character.ConstitutionSaveProficiency),
                    new Ability(c.Character.Intelligence, c.Character.IntelligenceSaveProficiency),
                    new Ability(c.Character.Wisdom, c.Character.WisdomSaveProficiency),
                    new Ability(c.Character.Charisma, c.Character.CharismaSaveProficiency),
                    c.Proficiencies.Cast<Skill.Type>().ToArray(), c.Character.ExperiencePoints
                ));

        }

        public static IEnumerable<CharacterListPoco> MapToListCharacter(this IQueryable<CharacterEntity> entities)
        {
            return entities.Select(c => new CharacterListPoco { Id = c.Id, Name = c.Name });
        }
    }
}