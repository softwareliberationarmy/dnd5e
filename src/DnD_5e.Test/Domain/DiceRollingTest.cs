using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DnD_5e.Test.Domain
{
    public class DiceRollingTest
    {
        [Theory]
        [InlineData("1d6", 1, 6)]
        [InlineData("2d6", 2, 12)]
        [InlineData("1d4", 1, 4)]
        public async Task Dice_Roll_Falls_In_Range(string diceRollRequest, int minValue, int maxValue)
        {
            var target = new DieRoller();
            var roll = await target.Roll(diceRollRequest);
            Console.WriteLine($"Rolls a {roll}");
            Assert.True(roll >= minValue && roll <= maxValue);
        }
    }

    public class DieRoller
    {
        private static Random _random = new Random();

        public async Task<int> Roll(string requestString)
        {
            var parsedRequest = await Parse(requestString);
            return await ProcessRoll(parsedRequest);
        }

        private async Task<int> ProcessRoll(DiceRollRequest parsedRequest)
        {
            int result = parsedRequest.Modifier;
            for (int i = 0; i < parsedRequest.Quantity; i++)
            {
                result += _random.Next(1, parsedRequest.Sides + 1);
            }

            return result;
        }

        private async Task<DiceRollRequest> Parse(string requestString)
        {
            int qty = 0, sides = 0, modifier = 0;

            var firstSplit = requestString.Split('d');
            if (firstSplit.Length == 2 && int.TryParse(firstSplit[0].Trim(), out qty))
            {
                if (firstSplit[1].Contains("+"))
                {
                    var secondSplit = firstSplit[1].Split('+');
                    if (secondSplit.Length == 2 
                        && int.TryParse(secondSplit[0].Trim(), out sides) 
                        && int.TryParse(secondSplit[1].Trim(), out modifier))
                    {
                        return new DiceRollRequest(qty, sides, modifier);
                    }
                }
                else if (firstSplit[1].Contains("-"))
                {
                    var secondSplit = firstSplit[1].Split('-');
                    if (secondSplit.Length == 2
                        && int.TryParse(secondSplit[0].Trim(), out sides)
                        && int.TryParse(secondSplit[1].Trim(), out modifier))
                    {
                        modifier *= -1;
                        return new DiceRollRequest(qty, sides, modifier);
                    }

                }
                else if(int.TryParse(firstSplit[1].Trim(), out sides))
                {
                    return new DiceRollRequest(qty, sides, modifier);
                }
            }

            throw new FormatException("Unable to parse roll request");
        }
    }

    internal class DiceRollRequest
    {
        public DiceRollRequest(in int qty, in int sides, in int modifier)
        {
            Quantity = qty;
            Sides = sides;
            Modifier = modifier;
        }

        public int Quantity { get; set; }
        public int Sides { get; set; }
        public int Modifier { get; set; }
    }
}
