namespace BattleshipEngine;

public struct ShipLocation
{
    public Ship Ship { get; private set; }
    public bool IsHorizontal { get; private set; }
    public MapCoordinates StartingTile { get; private set; }

    public ShipLocation(Ship newShip, MapCoordinates newCoordinates, bool newIsHorizontal)
    {
        Ship = newShip;
        StartingTile = newCoordinates;
        IsHorizontal = newIsHorizontal;
    }

    public void ChangeLocation(MapCoordinates newCoordinates, bool newIsHorizontal)
    {
        StartingTile = newCoordinates;
        IsHorizontal = newIsHorizontal;
    }

    public MapCoordinates[] GetCoordinatesFromShipLocation()
    {
        return MapCoordinates.GetAllCoordinates(StartingTile, IsHorizontal, Ship.Size);
    }

    public bool CheckIfTileIsOccupied(MapCoordinates coordinateToCheck)
    {
        var occupiedCoordinates = GetCoordinatesFromShipLocation();
        return occupiedCoordinates.Any(coordinateToCheck.Equals);
    }
}