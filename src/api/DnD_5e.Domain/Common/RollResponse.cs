using System.Linq;

namespace DnD_5e.Domain.Common
{
    public class RollResponse
    {
        public RollResponse(string requestedRoll, int roll)
        {
            Result = roll;
            Rolls = new[] {roll};
            RequestedRoll = requestedRoll;
        }

        public RollResponse(string requestedRoll, With rollType, int firstRoll, int secondRoll)
        {
            Rolls = new[] {firstRoll, secondRoll};
            Result = rollType == With.Advantage ? Rolls.Max() : Rolls.Min();
            RequestedRoll = requestedRoll;
        }

        public int Result { get; }
        // ReSharper disable once MemberCanBePrivate.Global
        public int[] Rolls { get; }
        public string RequestedRoll { get; }
    }
}