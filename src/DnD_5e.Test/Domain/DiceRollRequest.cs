namespace DnD_5e.Test.Domain
{
    internal class DiceRollRequest
    {
        public DiceRollRequest(in int qty, in int sides, in int modifier)
        {
            Quantity = qty;
            Sides = sides;
            Modifier = modifier;
        }

        public int Quantity { get; }
        public int Sides { get; }
        public int Modifier { get; }
    }
}