using BattleshipEngine.Maps;

namespace BattleshipEngine
{
    public struct ShipLocation
    {
        public Ship Ship { get; private set; }
        public bool IsHorizontal { get; private set; }
        public MapCoordinates StartingTile { get; private set; }

        private MapCoordinates[] _occupiedCoordinates;
        public MapCoordinates[] OccupiedCoordinates => _occupiedCoordinates;

        public ShipLocation(Ship newShip)
        {
            Ship = newShip;
            IsHorizontal = false;
            StartingTile = new MapCoordinates();

            _occupiedCoordinates = Array.Empty<MapCoordinates>();
        }

        public ShipLocation(ShipLocation oldShipLocation) : this(oldShipLocation.Ship)
        {
            Ship = oldShipLocation.Ship;
            StartingTile = oldShipLocation.StartingTile;
            IsHorizontal = oldShipLocation.IsHorizontal;

            UpdateOccupiedCoordinates();
        }

        public ShipLocation(Ship newShip, MapCoordinates newCoordinates, bool newIsHorizontal) : this(newShip)
        {
            Ship = newShip;
            StartingTile = newCoordinates;
            IsHorizontal = newIsHorizontal;

            UpdateOccupiedCoordinates();
        }

        private void UpdateOccupiedCoordinates()
        {
            _occupiedCoordinates = MapCoordinates.GetAllCoordinates(StartingTile, IsHorizontal, Ship.Size);
        }

        public void ChangeLocation(MapCoordinates newCoordinates, bool newIsHorizontal)
        {
            StartingTile = newCoordinates;
            IsHorizontal = newIsHorizontal;

            UpdateOccupiedCoordinates();
        }

        public bool CheckIfTileIsOccupied(MapCoordinates coordinateToCheck)
        {
            return _occupiedCoordinates.Any(coordinateToCheck.Equals);
        }
    }
}