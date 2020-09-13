using System.Linq;

namespace DnD_5e.Domain.Common
{
    public class RollResponse
    {
        public RollResponse(int roll)
        {
            Result = roll;
            Rolls = new[] {roll};
        }

        public RollResponse(With rollType, int firstRoll, int secondRoll)
        {
            Rolls = new[] {firstRoll, secondRoll};
            Result = rollType == With.Advantage ? Rolls.Max() : Rolls.Min();
        }

        public int Result { get; }
        // ReSharper disable once MemberCanBePrivate.Global
        public int[] Rolls { get; }
    }
}