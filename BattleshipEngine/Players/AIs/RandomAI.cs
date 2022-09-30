using BattleshipEngine.Maps;

namespace BattleshipEngine.Players.AIs
{
    public class RandomAI : BaseAI
    {
        public RandomAI(BattleshipSession battleshipSession) : base(battleshipSession)
        {
        }

        public override void PositionShips(ICreateMap ownMap)
        {
            foreach (var shipInMap in ownMap.ShipsInMap)
            {
                ownMap.PositionShipInRandomCoordinatesUnsafe(shipInMap);
            }
        }

        public override MapCoordinates GetFireCoordinates(ITargetMap enemyMap)
        {
            return GetRandomFireAtCoordinates(enemyMap);
        }
    }
}