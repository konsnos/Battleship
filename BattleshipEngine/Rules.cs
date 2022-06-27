namespace BattleshipEngine
{
    public struct Rules
    {
        public int ColumnsSize { get; }
        public int RowsSize { get; }
        public List<Ship> ShipsInMap { get; }

        public Rules(int newColumnsSize, int newRowsSize)
        {
            ColumnsSize = newColumnsSize;
            RowsSize = newRowsSize;
            ShipsInMap = new List<Ship>();
        }

        public void AddShip(string shipName, int size, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                ShipsInMap.Add(new Ship(shipName, size, ShipsInMap.Count));
            }
        }

        public static Rules Default()
        {
            var rules = new Rules(10, 10);
            rules.AddShip("Carrier", 5, 1);
            rules.AddShip("Battleship", 4, 1);
            rules.AddShip("Destroyer", 3, 1);
            rules.AddShip("Submarine", 3, 2);
            rules.AddShip("Patrol Boat", 2, 2);
            return rules;
        }
    }
}