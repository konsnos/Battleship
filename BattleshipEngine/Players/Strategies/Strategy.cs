using BattleshipEngine.Maps;

namespace BattleshipEngine.Players.Strategies
{
    public abstract class Strategy
    {
        public abstract void PositionShips(Map ownMap);
        public abstract MapCoordinates GetFireCoordinates(ITargetMap enemyMap);
    }
}