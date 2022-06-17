using BattleshipEngine.Maps;

namespace BattleshipEngine.Players
{
    public struct PlayerAction
    {
        public Player Player { get; }

        public Player EnemyPlayer { get; }
        public MapCoordinates FireCoordinates { get; }

        public PlayerAction(Player player, Player enemyPlayer, MapCoordinates fireCoordinates)
        {
            Player = player;
            EnemyPlayer = enemyPlayer;
            FireCoordinates = fireCoordinates;
        }
    }
}