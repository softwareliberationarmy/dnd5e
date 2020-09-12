using System;
using System.Linq;
using System.Threading.Tasks;
using DnD_5e.Domain.Common;

namespace DnD_5e.Domain.DiceRolls
{
    public class DieRoller
    {
        private static readonly Random _random = new Random();

        public async Task<RollResponse> Roll(string requestString)
        {
            var result = new RollResponse();
            var parsedRequest = await Parse(requestString);
            result.Rolls = new[]
            {
                await ProcessRoll(parsedRequest)
            };
            result.Result = result.Rolls.Single();
            return result;
        }

        private async Task<int> ProcessRoll(DiceRollRequest parsedRequest)
        {
            int result = parsedRequest.Modifier;
            for (int i = 0; i < parsedRequest.Quantity; i++)
            {
                result += _random.Next(1, parsedRequest.Sides + 1);
            }

            return await Task.FromResult(result);
        }

        private async Task<DiceRollRequest> Parse(string requestString)
        {
            int modifier = 0;

            var firstSplit = requestString.Split('d');
            if (firstSplit.Length == 2 && int.TryParse(firstSplit[0].Trim(), out var qty))
            {
                int sides;
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
                    return await Task.FromResult(new DiceRollRequest(qty, sides, modifier));
                }
            }

            throw new FormatException("Unable to parse roll request");
        }

        public async Task<RollResponse> RollWith(string rollRequest, With with)
        {
            var result = new RollResponse();
            var parsedRequest = await Parse(rollRequest);

            result.Rolls = new[]
            {
                await ProcessRoll(parsedRequest),
                await ProcessRoll(parsedRequest)
            };
            result.Result = with == With.Advantage ? result.Rolls.Max() : result.Rolls.Min();

            return result;
        }
    }
}