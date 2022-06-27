using BattleshipEngine.Maps;

namespace BattleshipEngine
{
    /// <summary>
    /// Defines a ship location that contain the ship, if it's horizontal or vertical,
    /// where does the placement start and which tiles contain it.
    /// </summary>
    public struct ShipLocation
    {
        public Ship Ship { get; }
        public bool IsHorizontal { get; private set; }
        public MapCoordinates StartingTile { get; private set; }

        public MapCoordinates[] OccupiedCoordinates { get; private set; }

        public ShipLocation(Ship newShip)
        {
            Ship = newShip;
            IsHorizontal = false;
            StartingTile = new MapCoordinates();

            OccupiedCoordinates = Array.Empty<MapCoordinates>();
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
            OccupiedCoordinates = MapCoordinates.GetCoordinates(StartingTile, IsHorizontal, Ship.Size);
        }

        public void ChangeLocation(MapCoordinates newCoordinates, bool newIsHorizontal)
        {
            StartingTile = newCoordinates;
            IsHorizontal = newIsHorizontal;

            UpdateOccupiedCoordinates();
        }

        public bool CheckIfTileIsOccupied(MapCoordinates coordinateToCheck)
        {
            return OccupiedCoordinates.Any(coordinateToCheck.Equals);
        }
    }
}