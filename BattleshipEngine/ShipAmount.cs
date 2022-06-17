namespace BattleshipEngine
{
    public struct ShipAmount
    {
        public Ship Ship { private set; get; }
        public int Amount { private set; get; }

        public ShipAmount(Ship ship, int amount)
        {
            Ship = ship;
            Amount = amount;
        }
    }
}