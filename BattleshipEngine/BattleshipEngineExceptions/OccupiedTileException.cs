using BattleshipEngine.Maps;

namespace BattleshipEngine.BattleshipEngineExceptions;

public class OccupiedTileException : Exception
{
    public MapCoordinates MapCoordinates { get; }
    public bool IsHorizontal { get; }
    public int ShipSize { get; }
    
    public OccupiedTileException(MapCoordinates newMapCoordinates, bool newIsHorizontal, int newShipSize) : 
        base($"Coordinates {newMapCoordinates}, size {newShipSize} and horizontal {newIsHorizontal} don't fit in positions")
    {
        MapCoordinates = newMapCoordinates;
        IsHorizontal = newIsHorizontal;
        ShipSize = newShipSize;
    }
}