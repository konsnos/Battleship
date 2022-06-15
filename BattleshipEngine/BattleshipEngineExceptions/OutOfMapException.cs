using BattleshipEngine.Maps;

namespace BattleshipEngine.BattleshipEngineExceptions;

public class OutOfMapException : Exception
{
    public MapCoordinates MapCoordinates;
    public bool IsHorizontal { private set; get; }
    public int ShipSize { private set; get; }

    public OutOfMapException(MapCoordinates newMapCoordinates, bool newIsHorizontal, int newShipSize) : base(
        $"Coordinates {newMapCoordinates}, size {newShipSize} and horizontal {newIsHorizontal} don't fit in positions")
    {
        MapCoordinates = newMapCoordinates;
        IsHorizontal = newIsHorizontal;
        ShipSize = newShipSize;
    }
}