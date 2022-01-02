namespace BattleshipEngine;

public struct Rules
{
    public int ColumnsSize { get; private set; }
    public int RowsSize { get; private set; }
    public Dictionary<Ship, int> shipsInMap;

    public Rules() : this(10, 10, new Dictionary<Ship, int>()
    {
        { new Ship("Carrier", 5), 1 },
        { new Ship("BattleshipEngine", 4), 1 },
        { new Ship("Destroyer", 3), 1 },
        { new Ship("Submarine", 3), 2 },
        { new Ship("Patrol Boat", 2), 2 }
    })
    {
    }

    public Rules(int newColumnsSize, int newRowsSize, Dictionary<Ship, int> newShipsInMap)
    {
        ColumnsSize = newColumnsSize;
        RowsSize = newRowsSize;
        shipsInMap = newShipsInMap;
    }
}