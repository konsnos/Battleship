namespace BattleshipEngine.Maps
{
    public interface ICreateMap
    {
        public IEnumerable<Ship> ShipsInMap { get; }
        void PositionShip(ShipLocation shipLocation);
        bool AreCoordinatesValid(MapCoordinates[] coordinatesArray);
        bool AreCoordinatesFree(MapCoordinates[] coordinatesArray);
        MapCoordinates GetRandomPositionForShip(bool isHorizontal, int size);
        ShipLocation PositionShipInRandomCoordinatesUnsafe(Ship ship);
        char[,] GetShipPositionsForPrint();
    }
}