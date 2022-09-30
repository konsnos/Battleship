using BattleshipEngine.Maps;

namespace BattleshipEngine.Players.AIs
{
    public abstract class BaseAI
    {
        public abstract void PositionShips(Map ownMap);
        public abstract MapCoordinates GetFireCoordinates(ITargetMap enemyMap);
    }
}