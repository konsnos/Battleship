namespace BattleshipEngine.Interfaces;

public interface IShipsMap
{
    public IEnumerable<Ship> ShipsInMap { get; }

    public void PositionShipInRandomCoordinatesUnsafe(ShipLocation newShipLocation);
    
    void PositionShip(ShipLocation newShipLocation);
    bool IsCoordinatesInsideMap(MapCoordinates[] coordinatesArray);
    bool IsCoordinatesFree(MapCoordinates[] coordinatesArray);
    bool IsShipPositioned(Ship ship, out ShipLocation positionedShip);
}