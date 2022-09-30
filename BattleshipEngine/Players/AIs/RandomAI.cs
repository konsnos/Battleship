using BattleshipEngine.Maps;

namespace BattleshipEngine.Players.AIs
{
    public class RandomAI : BaseAI
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