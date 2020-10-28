using System.Linq;

namespace DnD_5e.Domain.Common
{
    public class RollResponse
    {
        private RollResponse(string requestedRoll)
        {
            RequestedRoll = requestedRoll;
        }

        public RollResponse(string requestedRoll, int roll): this(requestedRoll)
        {
            Result = roll;
            Rolls = new[] {roll};
        }

        public RollResponse(string requestedRoll, With rollType, int firstRoll, int secondRoll): this(requestedRoll)
        {
            Rolls = new[] {firstRoll, secondRoll};
            Result = rollType == With.Advantage ? Rolls.Max() : Rolls.Min();
        }

        public int Result { get; }
        public int[] Rolls { get; }
        public string RequestedRoll { get; }
    }
}