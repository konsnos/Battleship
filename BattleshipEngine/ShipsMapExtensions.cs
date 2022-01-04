using BattleshipEngine.Interfaces;

namespace BattleshipEngine;

public static class ShipsMapExtensions
{
    public static void AddShipsRandomly(this IShipsMap shipsMap)
    {
        foreach (var shipInMap in shipsMap.ShipsInMap)
        {
            shipsMap.PositionShipInRandomCoordinatesUnsafe(new ShipLocation(shipInMap));
        }
    }
}