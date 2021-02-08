using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnD_5e.Domain.CharacterRolls;
using DnD_5e.Infrastructure.DataAccess;
using DnD_5e.Infrastructure.DataAccess.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DnD_5e.Test.UnitTests.Infrastructure
{
    public class CharacterRepositoryTests
    {
        public class GetById
        {
            [Fact]
            public async Task Returns_Null_When_Character_Not_Found()
            {
                var options = CreateNewInMemoryDatabase();

                await using var context = new CharacterDbContext(options.Options);

                var repo = new CharacterRepository(context);

                var character = repo.GetById(25);
                 
                character.Should().BeNull("No character found in database");
            }

            [Fact]
            public async Task Returns_character_with_correct_abilities()
            {
                var characterId = 223;
                var options = CreateNewInMemoryDatabase();

                await using (var context = new CharacterDbContext(options.Options))
                {
                    context.Character.Add(new CharacterEntity
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

                    var character = repo.GetById(characterId);
                    character.GetRoll(new CharacterRollRequest(Ability.Type.Strength, true))
                        .Should().Be("1d20p5");
                }
            }

            [Fact]
            public async Task Returns_character_with_correct_skill_proficiencies()
            {
                var characterId = 52352;
                var options = CreateNewInMemoryDatabase();

                await using (var context = new CharacterDbContext(options.Options))
                {
                    context.Character.Add(new CharacterEntity
                    {
                        Id = characterId,
                        Strength = 14,
                        SkillProficiencies = new List<SkillProficiencyEntity>
                    {
                        new SkillProficiencyEntity
                        {
                            Id = characterId + 1, Type = Skill.Type.Athletics
                        }
                    }
                    });

                    await context.SaveChangesAsync();
                }

                await using (var context = new CharacterDbContext(options.Options))
                {
                    var repo = new CharacterRepository(context);

                    var character = repo.GetById(characterId);
                    character.GetRoll(new CharacterRollRequest(Skill.Type.Athletics, Ability.Type.Strength))
                        .Should().Be("1d20p4");
                }
            }

            [Fact]
            public async Task Returns_character_with_correct_experience_points()
            {
                var characterId = 52352;
                var options = CreateNewInMemoryDatabase();

                await using (var context = new CharacterDbContext(options.Options))
                {
                    context.Character.Add(new CharacterEntity
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

                    var character = repo.GetById(characterId);
                    character.GetRoll(new CharacterRollRequest(Ability.Type.Wisdom, true))
                        .Should().Be("1d20p6");
                }
            }
        }

        public class GetCharactersByOwner
        {
            [Fact]
            public async Task Returns_all_characters_owned_by_user_name()
            {
                var userName = "google|123456789";

                var options = CreateNewInMemoryDatabase();
                await using (var context = new CharacterDbContext(options.Options))
                {
                    var user = new UserEntity {Name = userName};
                    context.User.Add(user);

                    await context.Character.AddRangeAsync(
                        new CharacterEntity
                        {
                            Id = 1,
                            Name = "Torvald the Elder",
                            Owner = user
                        },
                        new CharacterEntity
                        {
                            Id = 2,
                            Name = "Bravo the Chicken",
                            Owner = user
                        },
                        new CharacterEntity
                        {
                            Id = 3,
                            Name = "Fledgling Joe"
                        }
                        );

                    await context.SaveChangesAsync();
                }

                await using (var context = new CharacterDbContext(options.Options))
                {
                    var repo = new CharacterRepository(context);

                    var characters = repo.GetByOwner(userName);
                    characters.Count().Should().Be(2);
                }
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
