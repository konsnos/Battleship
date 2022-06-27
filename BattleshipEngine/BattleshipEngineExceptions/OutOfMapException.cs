using BattleshipEngine.Maps;

namespace BattleshipEngine.BattleshipEngineExceptions
{
    public class OutOfMapException : Exception
    {
        public MapCoordinates Coordinates { get; }
        public bool IsHorizontal { get; }
        public int ShipSize { get; }

        public OutOfMapException(MapCoordinates newMapCoordinates, bool newIsHorizontal, int newShipSize) : base(
            $"Coordinates {newMapCoordinates}, size {newShipSize} and horizontal {newIsHorizontal} don't fit in positions")
        {
            Coordinates = newMapCoordinates;
            IsHorizontal = newIsHorizontal;
            ShipSize = newShipSize;
        }
    }
}