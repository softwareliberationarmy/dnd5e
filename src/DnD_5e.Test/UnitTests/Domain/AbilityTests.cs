﻿using System;
using System.Collections.Generic;
using System.Text;
using DnD_5e.Domain.Roleplay;
using FluentAssertions;
using Xunit;

namespace DnD_5e.Test.UnitTests.Domain
{
    public class AbilityTests
    {
        [Theory]
        [InlineData(10, 0)]
        [InlineData(11, 0)]
        [InlineData(12, 1)]
        [InlineData(13, 1)]
        [InlineData(14, 2)]
        [InlineData(15, 2)]
        [InlineData(16, 3)]
        [InlineData(17, 3)]
        [InlineData(18, 4)]
        [InlineData(19, 4)]
        [InlineData(20, 5)]
        public void Returns_correct_modifier(int score, int expectedModifier)
        {
            var target = new Ability(score, false);
            target.GetAbilityModifier().Should().Be(expectedModifier);
        }
    }
}
