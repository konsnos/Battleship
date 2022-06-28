using BattleshipEngine.Maps;

namespace BattleshipEngine
{
    /// <summary>
    /// Contains the ship that was hit, which coordinate was hit last and if it's wrecked.
    /// </summary>
    public readonly struct ShipHitInfo
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