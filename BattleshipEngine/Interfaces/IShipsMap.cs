namespace BattleshipEngine.Interfaces;

public interface IShipsMap
{
    public IEnumerable<Ship> ShipsInMap { get; }

    public ShipLocation PositionShipInRandomCoordinatesUnsafe(Ship ship);
    
    void PositionShip(ShipLocation shipLocation);
    bool IsCoordinatesInsideMap(MapCoordinates[] coordinatesArray);
    bool IsCoordinatesFree(MapCoordinates[] coordinatesArray);
    bool IsShipPositioned(Ship ship, out ShipLocation positionedShip);
}