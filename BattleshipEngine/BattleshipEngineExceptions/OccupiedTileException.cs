namespace BattleshipEngine.BattleshipEngineExceptions;

public class OccupiedTileException : Exception
{
    public MapCoordinates MapCoordinates;
    public bool IsHorizontal { private set; get; }
    public int ShipSize { private set; get; }
    
    public OccupiedTileException(MapCoordinates newMapCoordinates, bool newIsHorizontal, int newShipSize) : 
        base($"Coordinates {newMapCoordinates}, size {newShipSize} and horizontal {newIsHorizontal} don't fit in positions")
    {
        MapCoordinates = newMapCoordinates;
        IsHorizontal = newIsHorizontal;
        ShipSize = newShipSize;
    }
}