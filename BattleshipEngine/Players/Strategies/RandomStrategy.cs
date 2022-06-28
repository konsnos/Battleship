using BattleshipEngine.Maps;

namespace BattleshipEngine.Players.Strategies
{
    public class RandomStrategy : Strategy
    {        
        private static readonly Random _random = new Random();

        public override void PositionShips(Map ownMap)
        {
            foreach (var shipInMap in ownMap.ShipsInMap)
            {
                ownMap.PositionShipInRandomCoordinatesUnsafe(shipInMap);
            }
        }

        public override MapCoordinates GetFireCoordinates(ITargetMap enemyMap)
        {
            var mapCoordinates = new MapCoordinates();
            do
            {
                mapCoordinates.ChangeCoordinates(_random.Next(enemyMap.Columns), _random.Next(enemyMap.Rows));
            } while (enemyMap.AreCoordinatesFiredAt(mapCoordinates));

            return mapCoordinates;
        }
    }
}