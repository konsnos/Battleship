namespace BattleshipEngine.Maps;

public interface ICreateMap
{
    void PositionShip(ShipLocation shipLocation);
    bool AreCoordinatesInsideMap(MapCoordinates[] coordinatesArray);
    bool AreCoordinatesFree(MapCoordinates[] coordinatesArray);
    MapCoordinates GetRandomPositionForShip(bool isHorizontal, int size);
    ShipLocation PositionShipInRandomCoordinatesUnsafe(Ship ship);
    char[,] GetShipPositionsForPrint();
}