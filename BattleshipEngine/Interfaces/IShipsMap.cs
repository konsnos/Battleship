namespace BattleshipEngine.Interfaces;

public interface IShipsMap
{
    void PositionShip(string newShipName, int newIndex, MapCoordinates newCoordinates, bool newIsHorizontal);
    bool IsCoordinatesInsideMap(MapCoordinates[] coordinatesArray);
    bool IsCoordinatesFree(MapCoordinates[] coordinatesArray);
}