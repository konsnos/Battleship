namespace BattleshipEngine;

public struct Rules
{
    public int ColumnsSize { get; }
    public int RowsSize { get; }
    public List<Ship> ShipsInMap { get; }

    public Rules()
    {
        ColumnsSize = 10;
        RowsSize = 10;
        ShipsInMap = new List<Ship>();
        
        AddShip("Carrier", 5, 1);
        AddShip("Battleship", 4, 1);
        AddShip("Destroyer", 3, 1);
        AddShip("Submarine", 3, 2);
        AddShip("Patrol Boat", 2, 2);
    }

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
}