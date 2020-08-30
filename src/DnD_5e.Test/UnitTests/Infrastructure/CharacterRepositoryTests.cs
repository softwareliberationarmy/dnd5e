using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DnD_5e.Domain.Roleplay;
using DnD_5e.Infrastructure.DataAccess;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DnD_5e.Test.UnitTests.Infrastructure
{
    public class CharacterRepositoryTests
    {
        [Fact]
        public async Task Returns_Null_When_Character_Not_Found()
        {
            var options = CreateNewInMemoryDatabase();

            var context = new CharacterDbContext(options.Options);

            var repo = new CharacterRepository(context);

            var character = await repo.GetById(25);

            character.Should().BeNull("No character found in database");
        }

        [Fact]
        public async Task Returns_character_with_correct_abilities()
        {
            var characterId = 223;
            var options = CreateNewInMemoryDatabase();

            await using (var context = new CharacterDbContext(options.Options))
            {
                context.Characters.Add(new CharacterEntity
                {
                    Id = characterId,
                    Strength = 16,
                    StrengthSaveProficiency = true
                });
                await context.SaveChangesAsync();
            }

            await using (var context = new CharacterDbContext(options.Options))
            {
                var repo = new CharacterRepository(context);

                var character = await repo.GetById(characterId);
                character.GetRoll(new CharacterRollRequest(Ability.Type.Strength, true))
                    .Should().Be("1d20+5");
            }
        }

        [Fact]
        public async Task Returns_character_with_correct_skill_proficiencies()
        {
            var characterId = 52352;
            var options = CreateNewInMemoryDatabase();

            await using (var context = new CharacterDbContext(options.Options))
            {
                context.Characters.Add(new CharacterEntity
                {
                    Id = characterId,
                    Strength = 14,
                    SkillProficiencies = new List<SkillProficiencyEntity>
                    {
                        new SkillProficiencyEntity
                        {
                            Id = characterId + 1, Type = (int) Skill.Type.Athletics
                        }
                    }
                });

                await context.SaveChangesAsync();
            }

            await using (var context = new CharacterDbContext(options.Options))
            {
                var repo = new CharacterRepository(context);

                var character = await repo.GetById(characterId);
                character.GetRoll(new CharacterRollRequest(Skill.Type.Athletics, Ability.Type.Strength))
                    .Should().Be("1d20+4");
            }
        }

        [Fact]
        public async Task Returns_character_with_correct_experience_points()
        {
            var characterId = 52352;
            var options = CreateNewInMemoryDatabase();

            await using (var context = new CharacterDbContext(options.Options))
            {
                context.Characters.Add(new CharacterEntity
                {
                    Id = characterId,
                    Wisdom = 14,
                    WisdomSaveProficiency = true,
                    ExperiencePoints = 50000
                });

                await context.SaveChangesAsync();
            }

            await using (var context = new CharacterDbContext(options.Options))
            {
                var repo = new CharacterRepository(context);

                var character = await repo.GetById(characterId);
                character.GetRoll(new CharacterRollRequest(Ability.Type.Wisdom, true))
                    .Should().Be("1d20+6");
            }
        }

        private static DbContextOptionsBuilder<CharacterDbContext> CreateNewInMemoryDatabase()
        {
            var options = new DbContextOptionsBuilder<CharacterDbContext>();
            options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            return options;
        }
    }
}
