namespace BattleshipEngine.Interfaces;

public interface IShipsMap
{
    void PositionShip(Ship newShip, MapCoordinates newCoordinates, bool newIsHorizontal);
    bool IsCoordinatesInsideMap(MapCoordinates[] coordinatesArray);
    bool IsCoordinatesFree(MapCoordinates[] coordinatesArray);
}