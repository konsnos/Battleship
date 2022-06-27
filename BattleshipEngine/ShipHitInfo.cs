using BattleshipEngine.Maps;

namespace BattleshipEngine
{
    public struct ShipHitInfo
    {
        public Ship Ship { get; }
        public MapCoordinates LastHit { get; }
        public bool IsShipWrecked { get; }

        public ShipHitInfo(Ship ship, MapCoordinates lastHit, bool shipWrecked)
        {
            Ship = ship;
            LastHit = lastHit;
            IsShipWrecked = shipWrecked;
        }
    }
}