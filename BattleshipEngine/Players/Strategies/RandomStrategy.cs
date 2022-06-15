using BattleshipEngine.Maps;

namespace BattleshipEngine.Players.Strategies;

public class RandomStrategy : Strategy
{
    public override void PositionShips(Map ownMap)
    {
        foreach (var shipInMap in ownMap.ShipsInMap)
        {
            ownMap.PositionShipInRandomCoordinatesUnsafe(shipInMap);
        }
    }

    public override MapCoordinates GetFireCoordinates(Map enemyMap)
    {
        MapCoordinates mapCoordinates;
        do
        {
            mapCoordinates = enemyMap.GetRandomCoordinates();
        } while (enemyMap.AreCoordinatesFiredAt(mapCoordinates));

        return mapCoordinates;
    }
}