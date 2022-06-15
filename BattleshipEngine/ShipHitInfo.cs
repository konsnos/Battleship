using BattleshipEngine.Maps;

namespace BattleshipEngine;

public struct ShipHitInfo
{
    public Ship Ship { private set; get; }
    public MapCoordinates LastHit { private set; get; }
    public bool IsShipWrecked { private set; get; }

    public ShipHitInfo(Ship ship, MapCoordinates lastHit, bool shipWrecked)
    {
        Ship = ship;
        LastHit = lastHit;
        IsShipWrecked = shipWrecked;
    }
}